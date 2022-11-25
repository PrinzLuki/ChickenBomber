using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class DamageAbleObject : BaseStats
{
    [SerializeField] int rewardPoints;
    [SerializeField] Vector3 popUpSpawnOffset;

    static public Action<int,Vector3> OnDestroyObject;

    public override void GetDmg(Rigidbody rb, float baseDmg)
    {
        CalculateDmg(out float dmg, rb);

        if (dmg == 0) return;
        health -= dmg + baseDmg;

        if (health <= 0)
        {
            OnDestroyObject?.Invoke(rewardPoints, transform.position + popUpSpawnOffset);
            Destroy(gameObject);
        }
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent<Rigidbody>(out Rigidbody rb) && other.gameObject.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            damageable.GetDmg(rb,GetDmgValue());
        }
    }
}
