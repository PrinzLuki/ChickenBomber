using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdMultipleProjectiles : BaseBird
{
    [SerializeField] GameObject projectilePrefab;

    private void Update()
    {
        if (!isAbilityEnabled) return;

        if (Input.GetMouseButtonDown(0))
        {
            UseAbility();
        }
    }

    public override void UseAbility()
    {
        base.UseAbility();

        //spawn projectiles

        //shoot projectiles in directions
    }


    protected override void OnCollisionEnter(Collision other)
    {
        base.OnCollisionEnter(other);
    }
}
