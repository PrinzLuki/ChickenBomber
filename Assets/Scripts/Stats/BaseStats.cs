using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStats : MonoBehaviour, IDamageable
{
    [SerializeField] float velocityDmgMultiplier;
    [SerializeField] float velocityThreshHold;
    [SerializeField] private float health;
    [SerializeField] private float attackDmg;

    public void GetDmg(Rigidbody rb)
    {
        CalculateDmg(out float dmg,rb);

        if (dmg == 0) return;

        health -= dmg;

        if(health <= 0)
        {
            Debug.Log("Health 0, " + gameObject.name + " is dead");
            Destroy(gameObject);
        }
    }

    void CalculateDmg(out float dmg,Rigidbody rb)
    { 
        dmg = 0;
        if (rb.velocity.magnitude * velocityDmgMultiplier < velocityThreshHold) return;
        dmg = rb.velocity.magnitude * velocityDmgMultiplier;

    }
    public float GetDmgValue()
    {
        return attackDmg;
    }
}
