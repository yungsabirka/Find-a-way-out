using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;

//A class that generates possible fields for the enemy to walk on
[RequireComponent(typeof(NavMeshSurface))]
public class NavMeshBuilder : MonoBehaviour, IInitializable
{
    private NavMeshSurface _navSurface;
    private bool _isInitialized;

    public bool IsInitialized => _isInitialized;

    public IEnumerator Initialize()
    {
        _navSurface = GetComponent<NavMeshSurface>();
        _navSurface.BuildNavMesh();
        _isInitialized = true;
        yield return null;
    }
}

