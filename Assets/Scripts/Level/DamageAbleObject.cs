using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DamageAbleObject : BaseStats
{
    [SerializeField] int rewardPoints;

    static public Action<int> OnDestroyObject;

    void OnDestroy()
    {
        OnDestroyObject?.Invoke(rewardPoints);
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent<Rigidbody>(out Rigidbody rb) && other.gameObject.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            damageable.GetDmg(rb,GetDmgValue());
        }
    }
}
