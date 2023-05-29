using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPickupable : MonoBehaviour
{
    [SerializeField] private GameObject[] pickupablePrefab;

    public static SpawnPickupable Instance;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void Spawn(Transform point)
    {
        int random = Random.Range(0, pickupablePrefab.Length);
        GameObject pickupable = Instantiate(pickupablePrefab[random], point.position, Quaternion.identity);
        pickupable.transform.position = new Vector3(pickupable.transform.position.x, pickupable.transform.position.y + 1.5f, pickupable.transform.position.z);

    }
}
