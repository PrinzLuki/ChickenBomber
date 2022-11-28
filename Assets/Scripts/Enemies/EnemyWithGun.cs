using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWithGun : DamageableEnemyEntity
{
    [SerializeField] Projectile projectile;
    [SerializeField] Transform projectileSpawnPoint;
    [SerializeField] float deleayBetweenProjectiles;
    [SerializeField] float projectileLifespan;
    [SerializeField] int numberOfProjectilesToSpawn;

    void Update()
    {
        UseAbility();
    }
    protected override void UseAbility()
    {
        if (canUseAbility)
        {
            StartCoroutine(SpawnProjectiles());
            StartCoroutine(AbilityCd());
        }
    }
    IEnumerator SpawnProjectiles()
    {
        WaitForSeconds wait = new WaitForSeconds(deleayBetweenProjectiles);

        for (int i = 0; i < numberOfProjectilesToSpawn; i++)
        {
            var instance = Instantiate(projectile,projectileSpawnPoint.position,Quaternion.identity);
            instance.transform.forward = projectileSpawnPoint.forward;
            Destroy(instance,projectileLifespan);
            yield return wait;
        }
    }
}
