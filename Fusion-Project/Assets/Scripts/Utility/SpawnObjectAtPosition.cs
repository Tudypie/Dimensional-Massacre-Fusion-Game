using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjectAtPosition : MonoBehaviour
{
    private Transform spawnPoint;
    public void SetPoint(Transform transform)
    {
        spawnPoint = transform;
    }
    public void SpawnObject(GameObject objectToSpawn)
    {
        Instantiate(objectToSpawn, spawnPoint.position, Quaternion.identity);
    }
}
