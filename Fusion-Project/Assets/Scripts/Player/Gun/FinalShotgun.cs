using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalShotgun : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private Bullets bullets;
    [SerializeField] private Rigidbody playerRigidbody;
    Animator anim;

    [Space]

    [Header("Effects")]
    [SerializeField, Space] private GameObject impactEffect;
    [SerializeField] private GameObject muzzleFlash;
    [SerializeField] private Transform muzzleFlashPosition;

    [Header("Stats")]
    [SerializeField, Space] Vector3 raycastOffset;
    [SerializeField] float range = 15f;
    [SerializeField] float damage = 20f;
    [SerializeField] float fireRate = 0.8f;
    float shootTimer;
    [SerializeField] float bulletsPerShot = 5f;
    [SerializeField] float inaccuracyDistance = 5f;
    [SerializeField, Space] bool shotgunPropulsion;
    [SerializeField] float propulsionForce = 12f;


    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if(shootTimer > 0f)
        {
            shootTimer -= Time.deltaTime;
            return;
        }

        if(!Input.GetButtonDown("Fire1"))
            return;

        FireShotgun();
    }

    public void FireShotgun()
    {
        shootTimer = fireRate;
        if(bullets.bullets <= 0)
        {
            AudioPlayer.Instance.PlayAudio(AudioPlayer.Instance.ammoPickup);
            return;
        }

        //GameObject muzzle = Instantiate(muzzleFlash, muzzleFlashPosition.position, Quaternion.identity);
        //Destroy(muzzle, 0.2f);

        anim.Play("Shoot");
        AudioPlayer.Instance.PlayAudio(AudioPlayer.Instance.shotgunShot);

        bullets.RemoveBullet();
        
        if(shotgunPropulsion)
            ApplyPropulsion();

        for(int i = 0; i < bulletsPerShot; i++)
        {
            RaycastHit hit;
            if(Physics.Raycast(transform.position + raycastOffset, GetShootingDirection(), out hit, range))
            {      
                if(hit.transform.CompareTag("Environment"))
                {
                    GameObject impact = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                    Destroy(impact, 0.2f);
                }

                if(hit.transform.GetComponent<Health>() != null)
                    hit.transform.GetComponent<Health>().TakeDamage(damage);
            }
        }
    }

    Vector3 GetShootingDirection()
    {
        Vector3 targetPos = transform.position + transform.forward * range;
        targetPos = new Vector3(
            targetPos.x + Random.Range(-inaccuracyDistance, inaccuracyDistance),
            targetPos.y + Random.Range(-inaccuracyDistance, inaccuracyDistance),
            targetPos.z + Random.Range(-inaccuracyDistance, inaccuracyDistance)
        );

        Vector3 shootingDirection = targetPos - transform.position;
        return shootingDirection.normalized;
    }
 
    private void ApplyPropulsion()
    {
        Vector3 launchDirection = -Camera.main.transform.forward;

        playerRigidbody.AddForce(launchDirection * propulsionForce, ForceMode.Impulse);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + raycastOffset, GetShootingDirection() * range);
    }


}
