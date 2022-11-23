using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdMultipleProjectiles : BaseBird
{
    [Header("Settings for Multiple Projectiles Bird")]
    [SerializeField] int projectileAmount;
    [SerializeField, Range(0, 1)] float startingProjectileDirection;
    [SerializeField, Range(-1, 0)] float endProjectileDirection;

    [Header("References and so stuff")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] List<GameObject> projectiles;


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

        float diff = startingProjectileDirection - endProjectileDirection;
        float step = diff / projectileAmount;
        float startRotX = transform.rotation.x + startingProjectileDirection;
        var myRot = transform.rotation;

        for (int i = 0; i < projectileAmount; i++)
        {
            Instantiate(projectilePrefab, transform.position, new Quaternion(startRotX, myRot.y, myRot.z, myRot.w));
            startRotX += step;
        }
    }


    protected override void OnCollisionEnter(Collision other)
    {
        base.OnCollisionEnter(other);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        //Gizmos.DrawRay(transform.position, startingProjectileDirection);

    }
}
