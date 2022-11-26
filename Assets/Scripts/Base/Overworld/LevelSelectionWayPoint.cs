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
    [SerializeField] bool isStartLevel;
    [SerializeField] bool isUnlocked;
    [SerializeField] bool isFinished;

    public event Action<LevelSelectionWayPoint> OnLevelSelected;

    #region Properties

    public bool IsStartLevel{ get => isStartLevel; }
    public bool IsFinished{ get => isFinished; set => isFinished = value; }
    public bool IsUnlocked { get => isUnlocked; set => isUnlocked = value; }
    #endregion

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
        if (!isUnlocked) return;
        OnLevelSelected?.Invoke(this);
    }
}
