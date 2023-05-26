﻿using System.Collections.Generic;
using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    private float speed = 5;

    [Header("Running")]
    public bool canRun = true;
    public bool IsRunning { get; private set; }
    public float runSpeed = 9;
    //public KeyCode runningKey = KeyCode.LeftShift;

    Rigidbody rb;
    /// <summary> Functions to override movement speed. Will use the last added override. </summary>
    public List<System.Func<float>> speedOverrides = new List<System.Func<float>>();



    void Awake()
    {
        // Get the rb on this.
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if(!canRun)
        {
            IsRunning = false;
            return;
        }

        IsRunning = true;
        float targetMovingSpeed = IsRunning ? runSpeed : speed;
        if (speedOverrides.Count > 0)
        {
            targetMovingSpeed = speedOverrides[speedOverrides.Count - 1]();
        }

        // Get targetVelocity from input.
        Vector2 targetVelocity =new Vector2( Input.GetAxis("Horizontal") * targetMovingSpeed, Input.GetAxis("Vertical") * targetMovingSpeed);

        // Apply movement.
        rb.velocity = transform.rotation * new Vector3(targetVelocity.x, rb.velocity.y, targetVelocity.y);
    }
}