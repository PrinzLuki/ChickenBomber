using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectionManager : MonoBehaviour
{
    [SerializeField] OverworldBird overworldBird;
    [SerializeField] LevelSelectionWayPoint currentlySelectedLevel;
    [SerializeField] LevelSelectionWayPoint[] allLevels;

    bool canLoadLevel = true;

    void Start()
    {
        SubscribeToEvents();
    }
    void OnDestroy()
    {
        UnSubscribeFromEvents();
    }

    public void LoadLevel()
    {
        if (overworldBird.CurrentWaypointStandingOn == null) return;

        SceneManager.LoadScene(overworldBird.CurrentWaypointStandingOn.GetSceneToLoad());
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
        Debug.Log(levelToReachIndex);
        Debug.Log(currentLevelIndex);
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
