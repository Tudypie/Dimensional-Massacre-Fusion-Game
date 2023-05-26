using UnityEngine;

public class TopDownCameraRotation : MonoBehaviour
{
    public float rotationSpeed = 10f;

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Mouse X");

        RotateCamera(horizontalInput);
    }

    private void RotateCamera(float rotationAmount)
    {
        float rotation = rotationAmount * rotationSpeed;

        transform.Rotate(Vector3.back, rotation);
    }
}
