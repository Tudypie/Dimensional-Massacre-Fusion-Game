using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Transform followTarget;
    [SerializeField] private Transform rotateTarget;
    [SerializeField] private float followSpeed = 5f;
    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, followTarget.position, followSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotateTarget.rotation, followSpeed * Time.deltaTime);
    }
}
