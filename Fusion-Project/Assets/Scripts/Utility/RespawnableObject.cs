using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnableObject : MonoBehaviour
{
    Vector3 startPosition;
    Vector3 startRotation;
    Rigidbody rb;

    private void Start()
    {
        startPosition = transform.position;
        startRotation = transform.eulerAngles;
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("FallTrigger"))
        {
            transform.position = startPosition;
            transform.eulerAngles = startRotation;
            rb.velocity = Vector3.zero;
        }
    }
}
