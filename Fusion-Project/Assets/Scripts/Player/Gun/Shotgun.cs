using UnityEngine;

public class Shotgun : MonoBehaviour
{
    [SerializeField] private float damage = 40f;
    [SerializeField] private float raycastDistance = 5f;
    [SerializeField] private float raycastRange = 5f;
    [SerializeField] private float shootDelay = 1.6f;
    private float shootTimer = 0f;
    public ParticleSystem muzzleFlash;
    public Camera mainCamera;

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
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

        RaycastHit[] hits = Physics.SphereCastAll(mainCamera.transform.position, raycastRange, mainCamera.transform.forward, raycastDistance);

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
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(mainCamera.transform.position + mainCamera.transform.forward * raycastDistance, raycastRange);
    }
}
