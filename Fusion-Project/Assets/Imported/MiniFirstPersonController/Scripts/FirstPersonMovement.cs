using System.Collections.Generic;
using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    public bool canMove { get; set;}
    public bool IsRunning { get; private set; }
    public float runSpeed = 22f;

    Rigidbody rb;
    public List<System.Func<float>> speedOverrides = new List<System.Func<float>>();

    void Awake()
    {   
        rb = GetComponent<Rigidbody>();

        canMove = true;
    }

    void FixedUpdate()
    {
        if(!canMove)
        {
            IsRunning = false;
            return;
        }

        IsRunning = true;
        float targetMovingSpeed = runSpeed;

        Vector2 targetVelocity =new Vector2( Input.GetAxis("Horizontal") * targetMovingSpeed, Input.GetAxis("Vertical") * targetMovingSpeed);

        rb.velocity = transform.rotation * new Vector3(targetVelocity.x, rb.velocity.y, targetVelocity.y);
    }
}