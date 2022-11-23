using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAbleObject : BaseStats
{
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent<Rigidbody>(out Rigidbody rb) && other.gameObject.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            damageable.GetDmg(rb,GetDmgValue());
        }
    }
}
