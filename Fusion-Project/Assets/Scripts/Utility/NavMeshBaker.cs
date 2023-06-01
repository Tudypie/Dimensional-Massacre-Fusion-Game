using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

public class NavMeshBaker : MonoBehaviour {

    public NavMeshSurface[] surfaces;
    public Transform[] objectsToRotate;
    public static NavMeshBaker Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        surfaces = FindObjectsOfType<NavMeshSurface>();
    }
    
    public void BakeNavigation() 
    {   
        for (int j = 0; j < objectsToRotate.Length; j++) 
        {
            objectsToRotate [j].localRotation = Quaternion.Euler (new Vector3 (0, Random.Range (0, 360), 0));
        }

        for (int i = 0; i < surfaces.Length; i++) 
        {
            surfaces [i].BuildNavMesh ();    
        }    
    }

}