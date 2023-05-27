using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Propulsion : MonoBehaviour
{   
    [SerializeField] private float launchForce = 10f;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void ApplyPropulsion()
    {
        Vector3 launchDirection = -Camera.main.transform.forward;

        rb.AddForce(launchDirection * launchForce, ForceMode.Impulse);
    }
}
