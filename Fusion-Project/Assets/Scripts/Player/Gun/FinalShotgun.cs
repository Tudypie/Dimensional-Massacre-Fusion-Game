using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalShotgun : MonoBehaviour
{

    [Header("References")]
    [SerializeField, Space] private Bullets bullets;
    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] Animator anim;
    [SerializeField] private MeshRenderer mr;

    [Space]

    [Header("Effects")]
    [SerializeField, Space] private GameObject impactEffect;
    [SerializeField] private GameObject Laser;
    [SerializeField] private Transform muzzleTransform;
    [SerializeField] private float laserFadeDuration = 0.3f;
    [SerializeField] private float laserMaxRange = 10f;
    private List<Vector3> activeLasersEndPoints = new List<Vector3>();
    private List<LineRenderer> activeLasers = new List<LineRenderer>();
    private float laserFadeTimer;

    [Header("Stats")]
    [SerializeField, Space] private bool shotgunEnabled;
    [SerializeField] Vector3 raycastOffset;
    [SerializeField] float range = 15f;
    [SerializeField] float damage = 20f;
    [SerializeField] float fireRate = 0.8f;
    float shootTimer;
    [SerializeField] float bulletsPerShot = 5f;
    [SerializeField] float inaccuracyDistance = 5f;
    [SerializeField, Space] bool shotgunPropulsion;
    [SerializeField] float propulsionForce = 12f;

    private void Update()
    {   
        if(laserFadeTimer > 0f)
        {
            laserFadeTimer -= Time.deltaTime;
            foreach(LineRenderer lr in activeLasers)
            {
                lr.SetPosition(0, muzzleTransform.position);
            }
        }

        if(!shotgunEnabled)
        {
            mr.enabled = false;
            return;
        }
        else
        {
            mr.enabled = true;
        }

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

        anim.Play("Shoot");
        AudioPlayer.Instance.PlayAudio(AudioPlayer.Instance.shotgunShot);

        bullets.RemoveBullet();
        
        if(shotgunPropulsion)
            ApplyPropulsion();

        for(int i = 0; i < bulletsPerShot; i++)
        {
            RaycastHit hit;
            Vector3 shootingDir = GetShootingDirection();
            if(Physics.Raycast(transform.position + raycastOffset, shootingDir, out hit, range))
            {    
                CreateLaser(hit.point);
                if(hit.transform.CompareTag("Environment"))
                {
                    GameObject impact = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                    Destroy(impact, 0.2f);
                }

                if(hit.transform.CompareTag("Enemy") && hit.transform.GetComponent<Health>() != null)
                    hit.transform.GetComponent<Health>().TakeDamage(damage);
            }
            else {
                CreateLaser(transform.position + shootingDir * range);
            }
        }
    }

    void CreateLaser(Vector3 endPoint)
    {
        laserFadeTimer = laserFadeDuration;

        LineRenderer lr = Instantiate(Laser, transform.position, Quaternion.identity).GetComponent<LineRenderer>();
        endPoint = Vector3.ClampMagnitude(endPoint - transform.position, laserMaxRange) + transform.position;
        lr.SetPositions(new Vector3[2] { muzzleTransform.position, endPoint });

        activeLasers.Add(lr);
        activeLasersEndPoints.Add(endPoint);

        StartCoroutine(FadeLaser(lr, endPoint));
    }

    IEnumerator FadeLaser(LineRenderer lr, Vector3 endPoint)
    {
        float alpha = 1f;
        while(alpha > 0f)
        {
            alpha -= Time.deltaTime / laserFadeDuration;
            lr.startColor = new Color(lr.startColor.r, lr.startColor.g, lr.startColor.b, alpha);
            lr.endColor = new Color(lr.startColor.r, lr.startColor.g, lr.startColor.b, alpha);
            yield return null;
        }
        activeLasers.Remove(lr);
        activeLasersEndPoints.Remove(endPoint);

        Destroy(lr.gameObject);
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

    public void EnableShotgun() => shotgunEnabled = true;
    public void DisableShotgun() => shotgunEnabled = false;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + raycastOffset, GetShootingDirection() * range);
    }


}
