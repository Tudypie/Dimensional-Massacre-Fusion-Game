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
    [SerializeField] LayerMask layerMask;

    [Header("Audio")]

    [SerializeField, Space] AudioClip beforeExplosionSound;
    [SerializeField] AudioClip explosionSound;

    private AudioSource audioSource;

    public float ExplosionTimer
    {
        get { return explosionTimer; }
        set { explosionTimer = value; }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Explode()
    {
        if(beforeExplosionSound != null)
            audioSource.PlayOneShot(beforeExplosionSound);

        Invoke("ExplodeAfterTimer", explosionTimer);
    }

    public void ExplodeAfterTimer()
    {   
        CameraShake.Instance.Shake(0.7f);
        if(explosionSound != null)
            AudioPlayer.Instance.PlayAudio(explosionSound, transform);
        Instantiate(explosionEffect, transform.position, transform.rotation);

        Debug.Log("Colliders");
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, layerMask);
        Debug.Log("Explosive has" + colliders.Length + " colliders in radius");
        for(int i = 0; i < colliders.Length; i++)
        {   
            if(colliders[i].gameObject == this.gameObject)
                continue;

            Debug.Log(colliders[i].name + " is in radius");
            if(colliders[i].GetComponent<Rigidbody>() != null)
            {
                Debug.Log(colliders[i].name + " has rigidbody");
                colliders[i].GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, radius);
            }

            if(colliders[i].GetComponent<Health>() != null)
            {
                Debug.Log(colliders[i].name + " has health");
                Health health = colliders[i].GetComponent<Health>();
                float distance = Vector3.Distance(transform.position, colliders[i].transform.position);
                float damage = explosionDamage / (distance/10);
                Debug.Log(colliders[i].name + " was " + distance + " units away and took " + damage + " damage.");
                
                if(colliders[i].tag == "Player")
                {
                    Debug.Log("Explosive damaged Player");
                    health.TakeDamage(damage*0.07f);
                }            
                else
                {
                    Debug.Log("Explosive damaged Enemy or something else");
                    health.TakeDamage(damage);
                }
            }
        }

        Destroy(gameObject);
    
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,radius);
    }

}
