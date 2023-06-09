using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalShotgun : MonoBehaviour
{

    [Header("References")]
    [SerializeField, Space] private LayerMask shootingRaycastLayers;
    [SerializeField] private Bullets bullets;
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

    [Header("Settings")]
    [SerializeField, Space] private bool shotgunEnabled;
    [SerializeField] Vector3 raycastOffset;
    [SerializeField] float range = 15f;
    [SerializeField] float damage = 20f;
    [SerializeField] float fireRate = 0.8f;
    float shootTimer;
    [SerializeField] float bulletsPerShot = 5f;
    [SerializeField] float inaccuracyDistance = 5f;
    [SerializeField, Space] bool shotgunPropulsion;
    [SerializeField] float propulsionForce = 100f;
    [SerializeField] float shootingForce = 100f;

    public float Damage { get => damage; set => damage = value; }
    public float FireRate { get => fireRate; set => fireRate = value; }

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

        if(Input.GetMouseButtonDown(0))
            FireShotgun();

    }

    public void FireShotgun()
    {

        Debug.Log(gameObject.name + " Fired");
        shootTimer = fireRate;
        if(bullets.bullets <= 0)
        {
            AudioPlayer.Instance.PlayAudio(AudioPlayer.Instance.ammoPickup);
            return;
        }

        anim.Play("Shoot");
        AudioPlayer.Instance.PlayAudio(AudioPlayer.Instance.shotgunShot, 0.5f);

        bullets.RemoveBullet();
        
        if(shotgunPropulsion)
            ApplyPropulsion();

        for(int i = 0; i < bulletsPerShot; i++)
            ShootRaycast();
    }

    public void ShootRaycast()
    {
        RaycastHit hit;
        Vector3 shootingDir = GetShootingDirection();

        if(Physics.Raycast(transform.position + raycastOffset, shootingDir, out hit, range, shootingRaycastLayers))
        {    
            //Debug.Log(hit.transform.name);

            CreateLaser(hit.point);

            if(hit.transform.CompareTag("Environment"))
            {
                GameObject impact = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impact, 0.2f);
            }

            if(hit.transform.GetComponent<Rigidbody>() != null)
                hit.transform.GetComponent<Rigidbody>().AddForceAtPosition(shootingDir * shootingForce, hit.point);

            if(hit.transform.GetComponent<Health>() != null && hit.transform.tag != "Player")
                hit.transform.GetComponent<Health>().TakeDamage(damage);
        }
        else 
        {
            CreateLaser(transform.position + shootingDir * range);
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

        if(Vector3.Angle(launchDirection, Vector3.up) < 45f)
            playerRigidbody.AddForce(launchDirection * propulsionForce);

    }

    public void EnableShotgun() => shotgunEnabled = true;
    public void DisableShotgun() => shotgunEnabled = false;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + raycastOffset, GetShootingDirection() * range);
    }


}
