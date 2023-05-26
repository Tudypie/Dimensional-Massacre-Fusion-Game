using UnityEngine;

public class RandomSpawn : MonoBehaviour
{
    public GameObject objectToSpawn;
    public Vector3 spawnBoxSize = new Vector3(10f, 10f, 10f);
    public float spawnInterval = 3f;

    private void Start()
    {
        InvokeRepeating("SpawnObject", 0f, spawnInterval);
    }

    private void SpawnObject()
    {
        Vector3 randomPosition = new Vector3(
            Random.Range(-spawnBoxSize.x / 2f, spawnBoxSize.x / 2f),
            Random.Range(-spawnBoxSize.y / 2f, spawnBoxSize.y / 2f),
            Random.Range(-spawnBoxSize.z / 2f, spawnBoxSize.z / 2f)
        );

        Vector3 spawnPosition = transform.position + randomPosition;

        Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, spawnBoxSize);
    }
}
