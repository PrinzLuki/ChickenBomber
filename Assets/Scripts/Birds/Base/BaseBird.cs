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
    [SerializeField] float OnReloadTriggerTime;
    bool isReloaded = false;
    public event Action OnReloadBird;
    public static event Action OnDestroyBird;
    public bool isLaunched = false;

    private void Awake()
    {
        stats = GetComponent<BaseStats>();
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezePositionZ;
        GetComponent<Collider>().enabled = false;
    }

    protected virtual void OnDestroy()
    {
        OnDestroyBird?.Invoke();

        if (!isReloaded)
        {
            OnReloadBird?.Invoke();
        }
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
            StartCoroutine(OnReloadTimeSpan());

        if (other.gameObject.GetComponent<IDamageable>() != null)
            other.gameObject.GetComponent<IDamageable>().GetDmg(rb,stats.GetDmgValue());
    }

    protected IEnumerator OnReloadTimeSpan()
    {
        isLaunched = false;
        yield return new WaitForSeconds(OnReloadTriggerTime);
        Debug.LogWarning("Deactivation TimeSpan Successfull Invoked");
        OnReloadBird?.Invoke();
        isReloaded = true;
    }

    public Rigidbody GetRb()
    {
        return rb;
    }

    public void SetisLaunched(bool isLaunched)
    {
        this.isLaunched = isLaunched;
        EnableAbility();
        GetComponent<Collider>().enabled = isLaunched;
        rb.useGravity = isLaunched;
    }

    protected void OnReloadDirectly()
    {
        OnReloadBird?.Invoke();
        isReloaded = true;
    }
}
