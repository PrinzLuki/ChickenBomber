using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageableEnemyEntity : DamageableEntity
{
    static public Action<int, Vector3, GameObject> OnDestroyEnemyObject;

    public override void GetDmg(Rigidbody rb, float baseDmg)
    {
        CalculateDmg(out float dmg, rb);

        if (dmg == 0) return;
        health -= dmg + baseDmg;

        if (health <= 0)
        {
            OnDestroyEnemyObject?.Invoke(rewardPoints, transform.position + popUpSpawnOffset, this.gameObject);
            Destroy(gameObject);
        }
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent<Rigidbody>(out Rigidbody rb) && other.gameObject.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            damageable.GetDmg(rb, GetDmgValue());
        }
    }
}
