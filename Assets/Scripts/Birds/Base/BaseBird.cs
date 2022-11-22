using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBird : MonoBehaviour
{
    BaseStats stats;

    private void Awake()
    {
        stats = GetComponent<BaseStats>();
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


}
