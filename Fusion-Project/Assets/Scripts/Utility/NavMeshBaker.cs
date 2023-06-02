using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

public class NavMeshBaker : MonoBehaviour {

    public static NavMeshBaker Instance;

    private void Awake()
    {
        Instance = this;
    }
    
    public void BakeNavigation(NavMeshSurface[] surfaces = null)
    {   
        for (int i = 0; i < surfaces.Length; i++) 
        {
            surfaces[i].BuildNavMesh();    
        }    
    }

    public void BakeNavigation(NavMeshSurface surface = null) 
    {   
        surface.BuildNavMesh();
    }
}