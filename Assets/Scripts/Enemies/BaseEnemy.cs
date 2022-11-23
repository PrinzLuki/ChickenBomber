using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody), typeof(EnemyStats))]
public class BaseEnemy : MonoBehaviour
{
    protected BaseStats stats;
    protected Rigidbody rb;

    private void Awake()
    {
        stats = GetComponent<BaseStats>();
        rb = GetComponent<Rigidbody>();
    }
}

