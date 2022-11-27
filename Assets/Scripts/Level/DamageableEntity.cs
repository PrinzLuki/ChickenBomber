using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class DamageableEntity : BaseStats
{
    [SerializeField] protected int rewardPoints;
    [SerializeField] protected Vector3 popUpSpawnOffset;

    static public Action<int,Vector3, GameObject> OnDestroyObject;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(2f);
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
    }
    public override void GetDmg(Rigidbody rb, float baseDmg)
    {
        CalculateDmg(out float dmg, rb);

        if (dmg == 0) return;
        health -= dmg + baseDmg;

        if (health <= 0)
        {
            OnDestroyObject?.Invoke(rewardPoints, transform.position + popUpSpawnOffset, this.gameObject);
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
