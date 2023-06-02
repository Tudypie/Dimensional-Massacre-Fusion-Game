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

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Explode()
    {
        audioSource.PlayOneShot(AudioPlayer.Instance.airRelease);
        Invoke("ExplodeAfterTimer", explosionTimer);
    }

    public void ExplodeAfterTimer()
    {   
        AudioPlayer.Instance.PlayAudio(AudioPlayer.Instance.explosion, transform);

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
