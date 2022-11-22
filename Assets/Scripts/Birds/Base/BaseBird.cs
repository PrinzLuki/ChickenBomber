using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(BirdStats))]
public class BaseBird : MonoBehaviour
{
    protected BaseStats stats;
    protected Rigidbody rb;

    private void Awake()
    {
        stats = GetComponent<BaseStats>();
        rb = GetComponent<Rigidbody>();
    }

    protected virtual void UseAbility()
    {
        Debug.Log("Standard Bird has no Ability");
    }

    protected virtual void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<IDamageable>() != null)
            other.gameObject.GetComponent<IDamageable>().GetDmg(stats.GetDmgValue());
    }
    
    public Rigidbody GetRb()
    {
        return rb;
    }
}
