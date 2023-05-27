using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewShotgun : MonoBehaviour
{
    [SerializeField] private LayerMask enemyLayerMask;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private float shootDelay = 0.8f;
    [SerializeField] private float maxDamage = 100f;
    [SerializeField] private float maxDistance = 10f;
    [SerializeField] private float minDistance = 3f;
    [SerializeField] private float spreadAngle = 30f;

    [SerializeField] private Bullets bullets;
    private float shootTimer = 0f;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();

        AudioPlayer.Instance.PlayAudio(AudioPlayer.Instance.shotgunEquip);
    }

    private void Update()
    {
    
        if (shootTimer > 0f)
            shootTimer -= Time.deltaTime;

        if (!Input.GetButtonDown("Fire1") || shootTimer > 0f)
            return;


        FireShotgun();

    }

    private void FireShotgun()
    {
        shootTimer = shootDelay;

        if(bullets.bullets <= 0)
        {
            AudioPlayer.Instance.PlayAudio(AudioPlayer.Instance.ammoPickup);
            return;
        }

        anim.Play("Shoot");
        AudioPlayer.Instance.PlayAudio(AudioPlayer.Instance.shotgunShot);

        bullets.RemoveBullet();

        Vector3 direction = transform.forward;
        Quaternion spreadRotation = Quaternion.AngleAxis(spreadAngle, transform.up);

        Vector3 leftSpreadDirection = spreadRotation * direction;
        Vector3 rightSpreadDirection = Quaternion.Inverse(spreadRotation) * direction;

        RaycastHit[] hitsLeft = Physics.RaycastAll(transform.position, leftSpreadDirection, maxDistance);
        RaycastHit[] hitsRight = Physics.RaycastAll(transform.position, rightSpreadDirection, maxDistance);

        foreach (RaycastHit hit in hitsLeft)
        {
            ProcessHit(hit);
        }

        foreach (RaycastHit hit in hitsRight)
        {
            ProcessHit(hit);
        }
    }

    private void ProcessHit(RaycastHit hit)
    {
        float distance = Vector3.Distance(transform.position, hit.point);

        float damage = CalculateDamage(distance);

        Health health = hit.transform.GetComponent<Health>();
        if(health != null)
        {
            health.TakeDamage(damage);
        }
    }



    private float CalculateDamage(float distance)
    {
        if (distance <= minDistance)
        {
            return maxDamage;
        }
        else if (distance >= maxDistance)
        {
            return 0f;
        }
        else
        {
            float damagePercentage = 1f - (distance - minDistance) / (maxDistance - minDistance);
            return maxDamage * damagePercentage;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Quaternion leftSpreadRotation = Quaternion.AngleAxis(-spreadAngle, transform.up);
        Quaternion rightSpreadRotation = Quaternion.AngleAxis(spreadAngle, transform.up);

        Vector3 leftSpreadDirection = leftSpreadRotation * transform.forward;
        Vector3 rightSpreadDirection = rightSpreadRotation * transform.forward;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, leftSpreadDirection * maxDistance);
        Gizmos.DrawRay(transform.position, rightSpreadDirection * maxDistance);
    }
}
