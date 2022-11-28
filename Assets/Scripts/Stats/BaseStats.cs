using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class BaseStats : MonoBehaviour, IDamageable
{
    [SerializeField] float velocityThreshHold;
    [SerializeField] protected float health;
    [SerializeField] private float attackDmg;

    async public virtual void GetDmg(Rigidbody rb,float baseDmg)
    {
        CalculateDmg(out float dmg,rb);
        if (dmg == 0) return;
        health -= dmg + baseDmg;
        
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    protected void CalculateDmg(out float dmg,Rigidbody rb)
    {
        dmg = 0;
        float magnitude = rb.velocity.magnitude;
        if (magnitude < velocityThreshHold) return;
        dmg = magnitude;
    }
    public float GetDmgValue()
    {
        return attackDmg;
    }
}
