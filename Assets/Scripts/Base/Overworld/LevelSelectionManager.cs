using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectionManager : MonoBehaviour
{
    [SerializeField] OverWorldRuntimeSaveData runtimeSaveData;
    [SerializeField] OverworldBird overworldBird;
    [SerializeField] LevelSelectionWayPoint currentlySelectedLevel;
    [SerializeField] LevelSelectionWayPoint[] allLevels;

    bool canLoadLevel = true;

    static public event Action<LevelSelectionWayPoint> SetCurrentLevel; 
    void Awake()
    {
       
    }
    void Start()
    {
        SubscribeToEvents();
        InitializeLevelStates();
        SetCurrentlySelectedLevel(allLevels[runtimeSaveData.levelBeingPlayed]);
        SetCurrentLevel?.Invoke(allLevels[runtimeSaveData.levelBeingPlayed]);
    }
    void OnDestroy()
    {
        UnSubscribeFromEvents();
    }
    void InitializeLevelStates()
    {
        for (int i = 0; i < allLevels.Length; i++)
        {
            if (!allLevels[i].IsStartLevel)
            {
                if (allLevels[i - 1].IsFinished && allLevels[i -1].IsUnlocked)
                {
                    allLevels[i].IsUnlocked = true;
                }
            }
            else if (allLevels[i].IsFinished)
            {
                allLevels[i + 1].IsUnlocked = true;
            }
        }
    }
    public void LoadLevel()
    {
        if (overworldBird.CurrentWaypointStandingOn == null) return;

        SetRuntimeSaveData();
        SceneManager.LoadScene(overworldBird.CurrentWaypointStandingOn.GetSceneToLoad());
    }
    void SetRuntimeSaveData()
    {
        runtimeSaveData.levelBeingPlayed = GetLevelIndex(currentlySelectedLevel);
        runtimeSaveData.lastPositionBird = allLevels[runtimeSaveData.levelBeingPlayed].transform.position;

        Vector3 cameraPos = new Vector3(allLevels[runtimeSaveData.levelBeingPlayed].transform.position.x, CameraController.Instance.transform.position.y, allLevels[runtimeSaveData.levelBeingPlayed].transform.position.z);
        runtimeSaveData.lastPositionCamera = cameraPos;
    }
    void SetCanLoadLevel(bool canLoadLevel)
    {
        this.canLoadLevel = canLoadLevel;
    }
    void SetCurrentlySelectedLevel(LevelSelectionWayPoint currentlySelectedLevel)
    {
        this.currentlySelectedLevel = currentlySelectedLevel;
    }
    void SubscribeToEvents()
    {
        overworldBird.IsLevelLoadable += SetCanLoadLevel;
        overworldBird.OnWaypointReached += SetCurrentlySelectedLevel;

        foreach (var levelSelectionWayPoint in allLevels)
        {
            levelSelectionWayPoint.OnLevelSelected += SetPathForOverworldBird;
        }
    }
    void UnSubscribeFromEvents()
    {
        overworldBird.IsLevelLoadable -= SetCanLoadLevel;
        overworldBird.OnWaypointReached -= SetCurrentlySelectedLevel;

        foreach (var levelSelectionWayPoint in allLevels)
        {
            levelSelectionWayPoint.OnLevelSelected -= SetPathForOverworldBird;
        }
    }
    void SetPathForOverworldBird(LevelSelectionWayPoint levelToReach)
    {
        StartCoroutine(overworldBird.WalkToLevel(GetPathToLevel(levelToReach), levelToReach));
    }
    Path[] GetPathToLevel(LevelSelectionWayPoint levelToReach)
    {
        int levelToReachIndex = GetLevelIndex(levelToReach);
        int currentLevelIndex = GetLevelIndex(currentlySelectedLevel);
        currentlySelectedLevel = levelToReach;
      
        bool levelIsBehindCurrentLevel = levelToReachIndex < currentLevelIndex;

        return GetPathsToSelectedLevel(currentLevelIndex, levelToReachIndex, levelIsBehindCurrentLevel);
    }
    int GetLevelIndex(LevelSelectionWayPoint leveToFind)
    {
        for (int i = 0; i < allLevels.Length; i++)
        {
            if (allLevels[i] == leveToFind)
            {
                return i;
            }
        }
        Debug.LogError("Level not added to Array" + leveToFind.GetSceneToLoad());
        return 0;
    }

    Path[] GetPathsToSelectedLevel(int currentLevelIndex,int levelToReachIndex,bool isBehindCurrentLevel)
    {
        if (currentLevelIndex == levelToReachIndex) return null;

        if (isBehindCurrentLevel)
        {
            Path[] paths = new Path[currentLevelIndex - levelToReachIndex];

            for (int i = currentLevelIndex,pathsI = 0; i > levelToReachIndex; i--,pathsI++)
            {
                paths[pathsI] = allLevels[i].GetPathToPreviousLevel();
            }
            return paths;
        }
        else
        {
            Path[] paths = new Path[levelToReachIndex - currentLevelIndex];

            for (int i = currentLevelIndex,pathsI = 0; i < levelToReachIndex; i++,pathsI++)
            {
                paths[pathsI] = allLevels[i].GetPathToNextLevel();
            }
            return paths;
        }
    }
}
