using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelSelectionWayPoint : MonoBehaviour
{
    [SerializeField] SceneAsset sceneToLoad;
    [SerializeField] Path pathToPreviousLevel;
    [SerializeField] Path pathToNextLevel;

    public event Action<LevelSelectionWayPoint> OnLevelSelected;

    public string GetSceneToLoad()
    {
        return sceneToLoad.name;
    }

    public Path GetPathToNextLevel()
    {
        return pathToNextLevel;
    }

    public Path GetPathToPreviousLevel()
    {
        return pathToPreviousLevel;
    }

    void OnMouseDown()
    {
        OnLevelSelected?.Invoke(this);
    }
}
