using UnityEngine;
using UnityEngine.Events;

public class SwitchPerspective : MonoBehaviour
{   
    private bool isTopDown = false;
    private bool finishedLerping = false;

    
    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private Transform playerTransform;

    private Quaternion initialTopDownRotation;
    [SerializeField] private Transform firstPersonTransform;
    [SerializeField] private Transform topDownTransform;

    [Space]

    [SerializeField] private UnityEvent onTopDownSwitch;
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
            finishedLerping = false;

            isTopDown = !isTopDown;
            mainCamera.orthographic = isTopDown;

            Cursor.visible = isTopDown;
            Cursor.lockState = isTopDown ? CursorLockMode.None : CursorLockMode.Locked;

            initialTopDownRotation = playerTransform.rotation;

            if (isTopDown)
            {   
                playerRigidbody.constraints |= RigidbodyConstraints.FreezePositionY;
                playerRigidbody.useGravity = false;
                onTopDownSwitch.Invoke();
            }
            else
            {
                playerRigidbody.constraints &= ~RigidbodyConstraints.FreezePositionY;
                playerRigidbody.useGravity = true;
                onFirstPersonSwitch.Invoke();
            }
        }

        if(isTopDown && playerTransform.rotation != initialTopDownRotation)
        {
            playerTransform.rotation = initialTopDownRotation;
        }

        if(finishedLerping)
            return;

        Transform targetTransform = isTopDown ? topDownTransform : firstPersonTransform;

        if(Vector3.Distance(transform.position, targetTransform.position) < 0.1f)
        {
            transform.position = targetTransform.position;
            transform.rotation = targetTransform.rotation;
            finishedLerping = true;
            return;
        }

        transform.position = Vector3.Lerp(transform.position, targetTransform.position, 40f * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetTransform.rotation, 40f * Time.deltaTime);
    }

}
