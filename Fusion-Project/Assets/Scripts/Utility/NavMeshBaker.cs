using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

public class NavMeshBaker : MonoBehaviour {

    public static NavMeshBaker Instance;

    [SerializeField] NavMeshSurface surface;

    private void Awake()
    {
        Instance = this;
    }
    public void BakeNavigation() 
    {   
        surface.BuildNavMesh();
    }
}