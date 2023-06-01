using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SwitchPerspective : MonoBehaviour
{       
    [SerializeField] private Texture2D crosshairTexture;

    [Header("Transforms")]
    [SerializeField, Space] private Transform playerTransform;
    [SerializeField] private Transform firstPersonCameraPoint;
    [SerializeField] private Transform topDownCameraPoint;
    [SerializeField] private Transform topDownParent;
    [SerializeField] List<Transform> surfacesTransform = new List<Transform>();
    private Transform targetTransform;

    [Header("Top Down Offset")]
    [SerializeField, Space] bool enableOffset = false;
    [SerializeField] float topDownCameraFollowSpeed = 5f;   
    [SerializeField] float topDownMaxOffset = 5f;   

    [Header("Top Down Transition")]
    [SerializeField, Space] float lerpTransitionSpeed = 5f;
    [SerializeField] float lerpTransitionDuration = 1f;
    private bool lerpTransitionInProgress = false;
    [SerializeField] private LayerMask groundLayer;

    private Vector3 topDownTargetPos;
    private Quaternion initialTopDownRotation;
    private bool isTopDown = false;
    private float switchTimer;


    [Space]

    [Header("Events")]
    [SerializeField, Space] private UnityEvent onTopDownSwitch;
    [SerializeField] private UnityEvent onTopDownFinishLerp;
    [SerializeField] private UnityEvent onFirstPersonSwitch;
    [SerializeField] private UnityEvent onFirstPersonFinishLerp;

    private Camera mainCamera;
    private Rigidbody playerRigidbody;

    public static SwitchPerspective Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        mainCamera = Camera.main;
        playerRigidbody = playerTransform.GetComponent<Rigidbody>();

        for(int i = 0; i < NavMeshBaker.Instance.surfaces.Length; i++)
        {
            surfacesTransform.Add(NavMeshBaker.Instance.surfaces[i].transform);
        }
    }

    private void Update()
    {
        if(lerpTransitionInProgress)
        {
            for(int i = 0; i < surfacesTransform.Count; i++)
            {
                surfacesTransform[i].localScale = Vector3.Lerp(surfacesTransform[i].localScale, 
                isTopDown ? new Vector3(1, 0.0001f, 1) : new Vector3(1, 1, 1),
                Time.deltaTime * lerpTransitionSpeed);
            }

            RaycastHit hit;
            Debug.DrawRay(new Vector3(topDownCameraPoint.position.x, 1000f, topDownCameraPoint.position.z), Vector3.down * 1000f, Color.blue, 5f);
            if(Physics.Raycast(new Vector3(topDownCameraPoint.position.x, 1000f, topDownCameraPoint.position.z),
            Vector3.down, out hit, Mathf.Infinity, groundLayer))
            {   
                playerTransform.position = new Vector3(playerTransform.position.x, hit.point.y + 1f, playerTransform.position.z);
            }

        }

        if(switchTimer > 0)
        {
            switchTimer -= Time.deltaTime;
            return;
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {   
            isTopDown = !isTopDown;
            mainCamera.orthographic = isTopDown;

            initialTopDownRotation = Quaternion.Euler(0f, RoundToNearestMultipleOf90(playerTransform.rotation.eulerAngles.y), 0f);
            
            targetTransform = isTopDown ? topDownCameraPoint : firstPersonCameraPoint;
            transform.rotation = targetTransform.rotation;
            transform.position = targetTransform.position;

            lerpTransitionInProgress = true;
            switchTimer = lerpTransitionDuration;
            Invoke("EndLerpingTransition", lerpTransitionDuration);

            if (isTopDown)
            {   
                playerRigidbody.constraints &= ~RigidbodyConstraints.FreezeRotationY;
                onTopDownSwitch.Invoke();
            }
            else
            {
                playerRigidbody.constraints |= RigidbodyConstraints.FreezeRotationY;
                onFirstPersonSwitch.Invoke();
            }
        }

        if(!isTopDown)
            return;

        if(playerTransform.rotation != initialTopDownRotation)
            playerTransform.rotation = initialTopDownRotation;
        
    }

    private void LateUpdate()
    {   
        if(!isTopDown)
            return;

        if(!enableOffset)
            return;

        topDownTargetPos = new Vector3(transform.position.x, targetTransform.position.y, transform.position.z);

        bool zDistance = Mathf.Abs(topDownTargetPos.z - topDownParent.position.z) > topDownMaxOffset;
        bool xDistance = Mathf.Abs(topDownTargetPos.x - topDownParent.position.x) > topDownMaxOffset;
        bool zDistanceForward = Mathf.Abs(topDownTargetPos.z + topDownParent.forward.z * topDownMaxOffset - topDownParent.position.z) > topDownMaxOffset;
        bool xDistanceForward = Mathf.Abs(topDownTargetPos.x + topDownParent.forward.x * topDownMaxOffset - topDownParent.position.x) > topDownMaxOffset;

       if ((zDistance || xDistance) && (zDistanceForward || xDistanceForward))
            return;
            
        topDownTargetPos += topDownParent.forward * topDownMaxOffset;

        transform.position = new Vector3(
        Mathf.Lerp(transform.position.x, topDownTargetPos.x, Time.deltaTime * topDownCameraFollowSpeed),
        topDownTargetPos.y,
        Mathf.Lerp(transform.position.z, topDownTargetPos.z, Time.deltaTime * topDownCameraFollowSpeed));

    }

    private float RoundToNearestMultipleOf90(float value)
    {
        float remainder = value % 90;

        float lowerMultiple = value - remainder;
        float upperMultiple = lowerMultiple + 90;

        if (remainder < 45)
        {
            return lowerMultiple;
        }
        else
        {
            return upperMultiple;
        }
    }

    private void EndLerpingTransition()
    {   
        if(isTopDown)
            onTopDownFinishLerp.Invoke();
        else
            onFirstPersonFinishLerp.Invoke();


        lerpTransitionInProgress = false;
        StopAllCoroutines();
        NavMeshBaker.Instance.BakeNavigation();

        RaycastHit hit;
        Debug.DrawRay(new Vector3(topDownCameraPoint.position.x, 1000f, topDownCameraPoint.position.z), Vector3.down * 1000f, Color.blue, 5f);
        if(Physics.Raycast(new Vector3(topDownCameraPoint.position.x, 1000f, topDownCameraPoint.position.z),
        Vector3.down, out hit, Mathf.Infinity, groundLayer))
        {   
            playerTransform.position = new Vector3(playerTransform.position.x, hit.point.y + 1f, playerTransform.position.z);
        }
    }

    private IEnumerator BakeNavigationOnSwitch()
    {
        while(lerpTransitionInProgress)
        {
            NavMeshBaker.Instance.BakeNavigation();
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void EnableMouse()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.SetCursor(crosshairTexture, Vector2.zero, CursorMode.Auto);
    }

    public void DisableMouse()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }


}
