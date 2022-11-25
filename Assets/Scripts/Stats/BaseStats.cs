using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BaseStats : MonoBehaviour, IDamageable
{
    [SerializeField] float velocityDmgMultiplier;
    [SerializeField] float velocityThreshHold;
    [SerializeField] protected float health;
    [SerializeField] private float attackDmg;

    public virtual void GetDmg(Rigidbody rb,float baseDmg)
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
        if (rb.velocity.magnitude < velocityThreshHold) return;
        dmg = rb.velocity.magnitude * velocityDmgMultiplier;

    }
    public float GetDmgValue()
    {
        return attackDmg;
    }
}
