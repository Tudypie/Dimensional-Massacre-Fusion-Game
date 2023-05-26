using UnityEngine;

public class Shotgun : MonoBehaviour
{
    [SerializeField] private float damage = 40f;
    [SerializeField] private float range = 5f;
    [SerializeField] private float shootDelay = 1.6f;
    private float shootTimer = 0f;
    public ParticleSystem muzzleFlash;
    public Camera mainCamera;

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

        AudioPlayer.Instance.PlayAudio(AudioPlayer.Instance.shotgunShot);

        RaycastHit hit;
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, range))
        {
            Debug.Log("Raycast hit: " + hit.transform.name);

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
        Gizmos.DrawRay(mainCamera.transform.position, mainCamera.transform.forward * range);
    }
}
