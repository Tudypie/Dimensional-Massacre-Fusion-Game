using UnityEngine;
using UnityEngine.Events;

public class SwitchPerspective : MonoBehaviour
{   
    [SerializeField] private bool isTopDown = false;
    private bool lerpingFinished = true;

    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform firstPersonTransform;
    [SerializeField] private Transform topDownTransform;

    private Quaternion initialTopDownRotation;

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
        topDownTransform.position = new Vector3(playerTransform.position.x, topDownTransform.position.y, playerTransform.position.z);

        if (Input.GetKeyDown(KeyCode.Tab))
        {   
            lerpingFinished = false;
            Invoke(nameof(LerpFinish), 0.5f);

            isTopDown = !isTopDown;
            mainCamera.orthographic = isTopDown;

            initialTopDownRotation = playerTransform.rotation;

            Cursor.visible = isTopDown;
            Cursor.lockState = isTopDown ? CursorLockMode.None : CursorLockMode.Locked;
            
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

        Transform targetTransform = isTopDown ? topDownTransform : firstPersonTransform;

        transform.position = new Vector3(transform.position.x, targetTransform.position.y, transform.position.z);

        if(lerpingFinished)
            return;
        transform.rotation = Quaternion.Lerp(transform.rotation, targetTransform.rotation, 40f * Time.deltaTime);
    }

    private void LerpFinish() => lerpingFinished = true;

}
