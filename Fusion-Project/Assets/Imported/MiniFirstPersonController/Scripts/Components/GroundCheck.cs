using UnityEngine;

[ExecuteInEditMode]
public class GroundCheck : MonoBehaviour
{
    public float radius = 0.5f;
    public LayerMask groundLayer;
    public bool isGrounded = true;
    public event System.Action Grounded;


    void LateUpdate()
    {
        bool isGroundedNow = CheckGround();

        if (isGroundedNow && !isGrounded)
        {
            Grounded?.Invoke();
        }

        isGrounded = isGroundedNow;
    }

    private bool CheckGround()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, groundLayer);

        if (colliders.Length > 0)
        {
            return true; 
        }

        return false; 
    }

    void OnDrawGizmosSelected()
    {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, radius);
    }
}
