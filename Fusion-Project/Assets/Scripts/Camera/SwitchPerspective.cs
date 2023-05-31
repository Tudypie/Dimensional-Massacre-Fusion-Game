using UnityEngine;
using UnityEngine.Events;

public class SwitchPerspective : MonoBehaviour
{   
    [SerializeField] private Texture2D crosshairTexture;

    [Header("Transforms")]
    [SerializeField, Space] private Transform playerTransform;
    [SerializeField] private Transform firstPersonTransform;
    [SerializeField] private Transform topDownTransform;
    [SerializeField] private Transform topDownParent;
    private Transform targetTransform;

    [Header("Top Down Settings")]
    [SerializeField, Space] float followSpeed = 5f;   
    [SerializeField] float offsetDistance = 5f;   
    private Vector3 topDownTargetPos;
    private Quaternion initialTopDownRotation;
    private bool isTopDown = false;

    [Space]

    [Header("Events")]
    [SerializeField, Space] private UnityEvent onTopDownSwitch;
    [SerializeField] private UnityEvent onFirstPersonSwitch;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {   
            isTopDown = !isTopDown;
            mainCamera.orthographic = isTopDown;

            Cursor.visible = isTopDown;
            Cursor.lockState = isTopDown ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.SetCursor(isTopDown ? crosshairTexture : default, Vector2.zero, CursorMode.Auto);

            initialTopDownRotation = Quaternion.Euler(0f, RoundToNearestMultipleOf90(playerTransform.rotation.eulerAngles.y), 0f);
            
            targetTransform = isTopDown ? topDownTransform : firstPersonTransform;
            transform.rotation = targetTransform.rotation;
            transform.position = targetTransform.position;

            if (isTopDown)
            {   
                onTopDownSwitch.Invoke();
            }
            else
            {
                onFirstPersonSwitch.Invoke();
            }
        }

        if(isTopDown && playerTransform.rotation != initialTopDownRotation)
            playerTransform.rotation = initialTopDownRotation;
    }

    private void LateUpdate()
    {
        if(!isTopDown)
            return;

        topDownTargetPos = new Vector3(transform.position.x, targetTransform.position.y, transform.position.z);

        bool zDistance = Mathf.Abs(topDownTargetPos.z - topDownParent.position.z) > offsetDistance;
        bool xDistance = Mathf.Abs(topDownTargetPos.x - topDownParent.position.x) > offsetDistance;
        bool zDistanceForward = Mathf.Abs(topDownTargetPos.z + topDownParent.forward.z * offsetDistance - topDownParent.position.z) > offsetDistance;
        bool xDistanceForward = Mathf.Abs(topDownTargetPos.x + topDownParent.forward.x * offsetDistance - topDownParent.position.x) > offsetDistance;

       if ((zDistance || xDistance) && (zDistanceForward || xDistanceForward))
            return;
            
        topDownTargetPos += topDownParent.forward * offsetDistance;

        transform.position = new Vector3(
        Mathf.Lerp(transform.position.x, topDownTargetPos.x, Time.deltaTime * followSpeed),
        topDownTargetPos.y,
        Mathf.Lerp(transform.position.z, topDownTargetPos.z, Time.deltaTime * followSpeed));

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

}
