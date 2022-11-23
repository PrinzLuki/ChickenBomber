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
    public event Action OnDestroyBird;
    public bool isLaunched = false;

    private void Awake()
    {
        stats = GetComponent<BaseStats>();
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezePositionZ;
    }

    protected void OnDestroy()
    {
        OnDestroyBird?.Invoke();
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
        if (isLaunched)
            StartCoroutine(DeactivationTimeSpan());

        if (other.gameObject.GetComponent<IDamageable>() != null)
            other.gameObject.GetComponent<IDamageable>().GetDmg(stats.GetDmgValue());
    }

    protected IEnumerator DeactivationTimeSpan()
    {
        yield return new WaitForSeconds(deActivationTime);
        Debug.Log("Deactivation TimeSpan Successfull Invoked");
        OnDeactivationBird?.Invoke();
    }

    public Rigidbody GetRb()
    {
        return rb;
    }

    public void SetisLaunched(bool isLaunched)
    {
        this.isLaunched = isLaunched;
    }
}
