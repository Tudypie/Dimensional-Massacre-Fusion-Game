using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Throwable : MonoBehaviour
{   
    [HideInInspector] public float throwForce;
    [SerializeField] UnityEvent OnThrow;
    [SerializeField] UnityEvent OnLand;

    bool landed = false;

    [Header("Audio")]
    [SerializeField, Space] AudioClip throwSound;
    [SerializeField] AudioClip landSound;

    private Rigidbody rb;
    private AudioSource audioSource;

    private void Start()
    {   
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * throwForce, ForceMode.Impulse);
        audioSource.PlayOneShot(throwSound);
        OnThrow?.Invoke();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Environment" || collision.gameObject.tag == "Enemy")
        {   
            if(landed)
                return;
            landed = true;
            OnLand?.Invoke();
            audioSource.PlayOneShot(landSound);
        }
    }


}
