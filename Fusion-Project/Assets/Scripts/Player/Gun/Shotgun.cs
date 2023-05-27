using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shotgun : MonoBehaviour
{   
    [SerializeField] private LayerMask enemyLayerMask;
    [SerializeField] private float damage = 40f;
    [SerializeField] private float raycastDistance = 5f;
    [SerializeField] private Vector3 raycastRange;
    [SerializeField] private float shootDelay = 1.6f;
    private float shootTimer = 0f;
    public ParticleSystem muzzleFlash;
    public Camera mainCamera;
    private Animator anim;

    private List<RaycastHit> hits = new List<RaycastHit>();

    private void Start()
    {
        anim = GetComponent<Animator>();
         
        AudioPlayer.Instance.PlayAudio(AudioPlayer.Instance.shotgunEquip);
    }


    private void Update()
    {   
        if(shootTimer > 0f)
            shootTimer -= Time.deltaTime;

        if (!Input.GetButtonDown("Fire1") || shootTimer > 0f)
            return;

        Shoot();

    }

    private void Shoot()
    {
        //muzzleFlash.Play();

        shootTimer = shootDelay;

        anim.Play("Shoot");

        AudioPlayer.Instance.PlayAudio(AudioPlayer.Instance.shotgunShot);

        Vector3 boxHalfExtents = new Vector3(raycastRange.x, raycastRange.y, raycastRange.z) * 0.5f;

        hits = new List<RaycastHit>(Physics.BoxCastAll(transform.position, boxHalfExtents, 
        Vector3.forward, transform.rotation, raycastDistance, enemyLayerMask));

        Debug.Log("Raycast hits: " + hits.Count);

        foreach (RaycastHit hit in hits)
        {
            Debug.Log("Raycast hit: " + hit.transform.name);

            if (hit.transform.CompareTag("Player"))
                return;

            Health health = hit.transform.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
        }

        foreach (RaycastHit hit in hits)
        {
            hits.Remove(hit);
        }

    }
private void OnDrawGizmosSelected()
{
    Vector3 boxHalfExtents = new Vector3(raycastRange.x, raycastRange.y, raycastRange.z) * 0.5f;

    Gizmos.color = Color.red;
    Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
    Gizmos.DrawWireCube(Vector3.zero, boxHalfExtents * 2f);
}


}
