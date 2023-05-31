using System.Collections.Generic;
using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    public bool canMove { get; set; }
    public bool IsRunning { get; private set; }
    public float runSpeed;
    public float damping = 25f;
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

    void Update()
    {
        if (!canMove)
        {
            IsRunning = false;
            return;
        }

        IsRunning = true;
        float targetMovingSpeed = runSpeed;

        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        float inputMagnitude = input.magnitude;
        inputMagnitude = Mathf.Clamp(inputMagnitude, 0f, 1f);
        input = input.normalized * inputMagnitude;

        Vector3 targetVelocity = transform.rotation * new Vector3(input.x * targetMovingSpeed, rb.velocity.y, input.y * targetMovingSpeed);

        rb.velocity = Vector3.Lerp(rb.velocity, targetVelocity, Time.deltaTime * damping);
    }

}
