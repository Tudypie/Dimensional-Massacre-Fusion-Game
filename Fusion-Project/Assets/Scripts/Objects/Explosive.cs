using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    [SerializeField] float radius = 5.0f;
    [SerializeField] float explosionForce = 700f;
    [SerializeField] float explosionDamage = 500f;
    [SerializeField] float explosionTimer = 1f;
    [SerializeField] GameObject explosionEffect;

    [Header("Audio")]

    [SerializeField, Space] AudioClip beforeExplosionSound;
    [SerializeField] AudioClip explosionSound;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Explode()
    {
        if(beforeExplosionSound != null)
            AudioPlayer.Instance.PlayAudio(beforeExplosionSound, transform);
        Invoke("ExplodeAfterTimer", explosionTimer);
    }

    private void ExplodeAfterTimer()
    {   
        CameraShake.Instance.Shake(0.7f);
        if(explosionSound != null)
            AudioPlayer.Instance.PlayAudio(explosionSound, transform);
        Instantiate(explosionEffect, transform.position, transform.rotation);

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach(Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, radius);
            }

            Health health = nearbyObject.GetComponent<Health>();
            if(health != null)
            {
                float distance = Vector3.Distance(transform.position, nearbyObject.transform.position);
                float damage = explosionDamage / (distance/3);
                Debug.Log(nearbyObject.name + " was " + distance + " units away and took " + damage + " damage.");
                
                if(nearbyObject.tag == "Player")
                    health.TakeDamage(damage*0.05f);
                else if(nearbyObject.tag == "Environment")
                    health.TakeDamage(damage * 0.5f);
                else
                    health.TakeDamage(damage);

            }
        }
    
        Destroy(gameObject);
    }

}
