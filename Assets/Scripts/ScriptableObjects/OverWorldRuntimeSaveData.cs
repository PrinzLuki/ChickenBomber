using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RuntimeSaveData" ,menuName = "NewRuntimeSaveData")]
public class OverWorldRuntimeSaveData : ScriptableObject
{
    public int levelBeingPlayed;
    public Vector3 lastPositionBird;
    public Vector3 lastPositionCamera;
}
