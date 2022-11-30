using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHandler : MonoBehaviour
{
    [SerializeField] GameObject[] hitParticles;
    [SerializeField] GameObject[] deathParticles;
    [SerializeField][Range(0,100)] int chanceToSpawnHitParticles = 50;
    [SerializeField] float effectDespawnDelay;
    [SerializeField] float timeBetweenOnHitEffects;

    float timeSinceLastOnHitSpawned = 1f;

    void Start()
    {
        DamageableEntity.OnDestroyObject += SpawnDeathParticles;
    }

    void OnDestroy()
    {
        DamageableEntity.OnDestroyObject -= SpawnDeathParticles;
    }
    GameObject GetRandomParticleEffect(GameObject[] effectsToChooseFrom)
    {
        int randomIndex = Random.Range(0,effectsToChooseFrom.Length);
        return effectsToChooseFrom[randomIndex];
    }
    void SpawnDeathParticles(int _,Vector3 position,GameObject owner)
    {
        if (owner == gameObject)
        {
            var instance = Instantiate(GetRandomParticleEffect(deathParticles), transform.position, Quaternion.identity);
            Destroy(instance,effectDespawnDelay);
        }
        
    }

    bool IsChanceHighEnoughToSpawnParticles()
    {
        return Random.Range(0, 100) <= chanceToSpawnHitParticles;
    }
    void OnCollisionEnter(Collision other)
    {
        if (timeSinceLastOnHitSpawned < Time.time && IsChanceHighEnoughToSpawnParticles())
        {
            timeSinceLastOnHitSpawned = Time.time + timeBetweenOnHitEffects;
            var instance = Instantiate(GetRandomParticleEffect(hitParticles), transform.position, Quaternion.identity);
            Destroy(instance,effectDespawnDelay);
        }
    }
}
