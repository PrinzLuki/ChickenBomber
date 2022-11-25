using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldBird : MonoBehaviour
{
    [SerializeField] LevelSelectionWayPoint currentWaypointStandingOn;
    [SerializeField] float walkingSpeed;

    public event Action<LevelSelectionWayPoint> OnWaypointReached;
    public event Action<bool> IsLevelLoadable; 
    bool canWalk = true;

    #region Properties

    public LevelSelectionWayPoint CurrentWaypointStandingOn{ get => currentWaypointStandingOn; }

    #endregion

    public IEnumerator WalkToLevel(Path[] pathsToWalk,LevelSelectionWayPoint wayPointToReach)
    {
        if (canWalk == false || pathsToWalk == null) yield break;

        WaitForEndOfFrame wait = new WaitForEndOfFrame();

        canWalk = false;

        IsLevelLoadable?.Invoke(canWalk);

        Debug.Log("StartedWalkingPaths");

        for (int i = 0; i < pathsToWalk.Length; i++)
        {
            Debug.Log(pathsToWalk[i]);
            Transform[] waypointPositions = pathsToWalk[i].GetPathWayPoints();

            for (int j = 0; j < waypointPositions.Length; j++)
            {
                while (waypointPositions[j].position != transform.position)
                {
                    transform.position = Vector3.MoveTowards(transform.position, waypointPositions[j].position, walkingSpeed * Time.deltaTime);

                    Debug.Log("WalkingPath");

                    yield return wait;
                }
            }
        }

        canWalk = true;
        IsLevelLoadable?.Invoke(canWalk);
        currentWaypointStandingOn = wayPointToReach;
        OnWaypointReached?.Invoke(wayPointToReach);
    }
}
