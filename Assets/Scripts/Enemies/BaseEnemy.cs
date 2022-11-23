using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody), typeof(EnemyStats))]
public class BaseEnemy : MonoBehaviour
{
    protected BaseStats stats;
    protected Rigidbody rb;

    [Header("Points")]
    [SerializeField] private int rewardPoints;
    [SerializeField] private Vector3 spawnOffset;
    [SerializeField] private static event Action<int, Vector3> OnDestroyObject;

    private void OnDestroy()
    {
        OnDestroyObject?.Invoke(rewardPoints, transform.position + spawnOffset);
    }

    private void Awake()
    {
        stats = GetComponent<BaseStats>();
        rb = GetComponent<Rigidbody>();
    }
}

