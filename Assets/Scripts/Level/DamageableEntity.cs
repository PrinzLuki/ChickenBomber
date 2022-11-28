using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof (Rigidbody))]
public class DamageableEntity : BaseStats
{
    [SerializeField] protected int rewardPoints;
    [SerializeField] protected Vector3 popUpSpawnOffset;

    static public Action<int,Vector3, GameObject> OnDestroyObject;

    Rigidbody rb;
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    public override void GetDmg(Rigidbody rb, float baseDmg)
    {
        CalculateDmg(out float dmg, rb);
        if (dmg == 0) return;
        rb.drag = 0.1f;
        health -= dmg + baseDmg;

        if (health <= 0)
        {
            OnDestroyObject?.Invoke(rewardPoints, transform.position + popUpSpawnOffset, this.gameObject);
            Destroy(gameObject);
        }
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent<Rigidbody>(out Rigidbody rb) && other.gameObject.TryGetComponent(out IDamageable damageable))
        {
            rb.constraints = RigidbodyConstraints.FreezePositionZ;
            damageable.GetDmg(rb, GetDmgValue());
        }
    }
}
