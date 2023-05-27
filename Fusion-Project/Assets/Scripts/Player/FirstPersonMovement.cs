using System.Collections.Generic;
using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    public bool canMove { get; set; }
    public bool IsRunning { get; private set; }
    public float runSpeed;
    public float damping = 10f;
    public float RunSpeed
    {
        get { return runSpeed; }
        set { runSpeed = value; }
    }
    Rigidbody rb;
    public List<System.Func<float>> speedOverrides = new List<System.Func<float>>();

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        canMove = true;
    }

    void FixedUpdate()
    {
        if (!canMove)
        {
            IsRunning = false;
            return;
        }

        IsRunning = true;
        float targetMovingSpeed = runSpeed;

        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Vector2 normalizedInput = input.normalized;
        Vector3 targetVelocity = transform.rotation * new Vector3(normalizedInput.x * targetMovingSpeed, rb.velocity.y, normalizedInput.y * targetMovingSpeed);

        rb.velocity = Vector3.Lerp(rb.velocity, targetVelocity, Time.fixedDeltaTime * damping);
    }
}
