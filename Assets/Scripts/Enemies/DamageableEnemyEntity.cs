using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DamageableEnemyEntity : DamageableEntity
{
    [SerializeField] protected float abilityCd;
    [SerializeField] protected bool canUseAbility = true;

    protected void SetCanUseAbility(bool canUseAbility)
    {
        this.canUseAbility = canUseAbility;
    }
    protected abstract void UseAbility();
    protected IEnumerator AbilityCd()
    {
        canUseAbility = false;
        yield return new WaitForSeconds(abilityCd);
        canUseAbility = true;
    }
}
