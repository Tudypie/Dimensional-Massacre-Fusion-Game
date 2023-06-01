using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] objectToSpawn;
    public Vector3 spawnBoxSize = new Vector3(10f, 10f, 10f);
    public float spawnInterval = 3f;

    private void OnEnable()
    {
        StartCoroutine(SpawnObjectCoroutine());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator SpawnObjectCoroutine()
    {
        while(true)
        {
            SpawnObject();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnObject()
    {
        Vector3 randomPosition = new Vector3(
            Random.Range(-spawnBoxSize.x / 2f, spawnBoxSize.x / 2f),
            Random.Range(-spawnBoxSize.y / 2f, spawnBoxSize.y / 2f),
            Random.Range(-spawnBoxSize.z / 2f, spawnBoxSize.z / 2f)
        );

        Vector3 spawnPosition = transform.position + randomPosition;

        int random = Random.Range(0, objectToSpawn.Length);
        GameObject spawnedObject = Instantiate(objectToSpawn[random], spawnPosition, Quaternion.identity);

        if(spawnedObject.GetComponent<ChaserMonster>() != null)
        {
            spawnedObject.GetComponent<ChaserMonster>().chaseDistance = 150f;
            spawnedObject.GetComponent<ChaserMonster>().dropPickupChance = 2;
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, spawnBoxSize);
    }
}
