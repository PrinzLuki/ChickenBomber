using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Path 
{
    [SerializeField] Transform[] pathWayPoints;

    public Transform[] GetPathWayPoints()
    {
        return pathWayPoints;
    }
}
