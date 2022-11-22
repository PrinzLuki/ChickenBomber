using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(BirdStats))]
public class BaseBird : MonoBehaviour
{
    protected BaseStats stats;
    protected Rigidbody rb;
    protected bool isAbilityEnabled = false;

    protected float deActivationTime;

    public event Action OnDeactivationBird;

    private void Awake()
    {
        stats = GetComponent<BaseStats>();
        rb = GetComponent<Rigidbody>();
    }

    public virtual void EnableAbility()
    {
        isAbilityEnabled = true;
        Debug.Log("Enabled Ability");
    }

    public virtual void UseAbility()
    {
        Debug.Log("Used Ability");
        DisableAbility();
    }

    public virtual void DisableAbility()
    {
        isAbilityEnabled = false;
        Debug.Log("Disabled Ability");
    }

    protected virtual void OnCollisionEnter(Collision other)
    {


        if (other.gameObject.GetComponent<IDamageable>() != null)
            other.gameObject.GetComponent<IDamageable>().GetDmg(stats.GetDmgValue());
    }
    
    protected IEnumerator DeactivationTimeSpan()
    {
        yield return new WaitForSeconds(deActivationTime);
        OnDeactivationBird?.Invoke();
    }

    public Rigidbody GetRb()
    {
        return rb;
    }
}
