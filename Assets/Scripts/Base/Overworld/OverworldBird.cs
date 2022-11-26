using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldBird : MonoBehaviour
{
    [SerializeField] OverWorldRuntimeSaveData runtimeSaveData;
    [SerializeField] LevelSelectionWayPoint currentWaypointStandingOn;
    [SerializeField] float walkingSpeed;

    public event Action<LevelSelectionWayPoint> OnWaypointReached;
    public event Action<bool> IsLevelLoadable; 
    bool canWalk = true;

    #region Properties

    public LevelSelectionWayPoint CurrentWaypointStandingOn{ get => currentWaypointStandingOn; }

    #endregion

    void Awake()
    {
        LevelSelectionManager.SetCurrentLevel += SetCurrentWaypointStandingOn;
    }
    void Start()
    {
        transform.position = runtimeSaveData.lastPositionBird;
    }

    void OnDestroy()
    {
        LevelSelectionManager.SetCurrentLevel -= SetCurrentWaypointStandingOn;
    }
    void SetCurrentWaypointStandingOn(LevelSelectionWayPoint currentLevelWaypoint)
    {
        currentWaypointStandingOn = currentLevelWaypoint;
    }
    public IEnumerator WalkToLevel(Path[] pathsToWalk,LevelSelectionWayPoint wayPointToReach)
    {
        if (canWalk == false || pathsToWalk == null) yield break;

        WaitForEndOfFrame wait = new WaitForEndOfFrame();

        canWalk = false;

        IsLevelLoadable?.Invoke(canWalk);

        for (int i = 0; i < pathsToWalk.Length; i++)
        {
            
            Transform[] waypointPositions = pathsToWalk[i].GetPathWayPoints();

            for (int j = 0; j < waypointPositions.Length; j++)
            {
                transform.forward = (waypointPositions[j].position - transform.position).normalized;

                while (waypointPositions[j].position != transform.position)
                {
                    transform.position = Vector3.MoveTowards(transform.position, waypointPositions[j].position, walkingSpeed * Time.deltaTime);

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
