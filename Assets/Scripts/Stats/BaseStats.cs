using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStats : MonoBehaviour, IDamageable
{
    [SerializeField] private float health;
    [SerializeField] private float attackDmg;

    public void GetDmg(float dmg)
    {
        health -= dmg;

        if(health <= 0)
        {
            Debug.Log("Health 0, " + gameObject.name + " is dead");
            Destroy(gameObject);
        }
    }

    public float GetDmgValue()
    {
        return attackDmg;
    }
}
