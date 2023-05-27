using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shotgun : MonoBehaviour
{   
    [SerializeField] private LayerMask enemyLayerMask;
    [SerializeField] private float damage = 40f;
    [SerializeField] private Vector3 raycastOffset;
    [SerializeField] private Vector3 raycastRange;
    [SerializeField] private float shootDelay = 1.6f;
    private float shootTimer = 0f;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private Transform mainCamera;
    private Animator anim;

    private List<RaycastHit> hits = new List<RaycastHit>();

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {   
        if (shootTimer > 0f)
            shootTimer -= Time.deltaTime;

        if (!Input.GetButtonDown("Fire1") || shootTimer > 0f)
            return;

        Shoot();
    }

    private void Shoot()
    {
        shootTimer = shootDelay;

        anim.Play("Shoot");

        AudioPlayer.Instance.PlayAudio(AudioPlayer.Instance.shotgunShot);

        Vector3 boxHalfExtents = new Vector3(raycastRange.x, raycastRange.y, raycastRange.z) * 0.5f;
        Vector3 offset = new Vector3(raycastOffset.x, raycastOffset.y, raycastOffset.z); 
        Vector3 startPosition = transform.position + offset;

        hits = new List<RaycastHit>(Physics.BoxCastAll(startPosition, boxHalfExtents, 
            mainCamera.transform.forward, transform.rotation, enemyLayerMask));

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

        hits.Clear();
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 boxHalfExtents = new Vector3(raycastRange.x, raycastRange.y, raycastRange.z) * 0.5f;
        Vector3 offset = new Vector3(raycastOffset.x, raycastOffset.y, raycastOffset.z); 
        Vector3 startPosition = transform.position + offset;

        Gizmos.color = Color.red;
        Gizmos.matrix = Matrix4x4.TRS(startPosition, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, boxHalfExtents * 2f);
    }
}
