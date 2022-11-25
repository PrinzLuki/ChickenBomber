using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingBird : BaseBird
{
    [SerializeField] GameObject birdChild;
    [SerializeField] ParticleSystem explosionEffect;
    [SerializeField, Tooltip("The Amount of time to destroy this oject after the ability")] float lifeTimeSpan;

    private void Update()
    {
        if (!isAbilityEnabled) return;

        if (Input.GetMouseButtonDown(0))
        {
            UseAbility();
        }
    }

    //Exploding Ability
    public override void UseAbility()
    {
        base.UseAbility();
        birdChild.SetActive(false);
        Debug.Log("Explosion Ability");
        explosionEffect.Play();
        GetComponent<Collider>().enabled = false;
        Destroy(gameObject, lifeTimeSpan);
        //Effects play for exploding
    }


    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (isLaunched) OnReloadDirectly();
    }

    protected override void OnCollisionEnter(Collision other)
    {
        base.OnCollisionEnter(other);
        UseAbility();
    }
}
