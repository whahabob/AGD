using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshBaker : MonoBehaviour
{
    private NavMeshSurface[] surfaces;
    
	void Update ()
    {
		if (surfaces == null || surfaces.Length == 0)
        {
            surfaces = GetComponentsInChildren<NavMeshSurface>();

            for (int i = 0; i < surfaces.Length; i++)
            {
                surfaces[i].BuildNavMesh();
            }
        }
	}
}
