using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdBoomerang : BaseBird
{
    [SerializeField] float timeSpanBoomerangForce;
    [SerializeField] Vector3 boomerangDirection;
    [SerializeField] float forceMultiplier;
    Coroutine routine;
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
        
        routine = StartCoroutine(OnAbilityTimeSpan());
    }

    protected override void OnCollisionEnter(Collision other)
    {
        base.OnCollisionEnter(other);

        DisableAbility();

        if(routine == null) return;
        StopCoroutine(routine);
    }

    IEnumerator OnAbilityTimeSpan()
    {
        float timer = timeSpanBoomerangForce;
        WaitForEndOfFrame wait = new WaitForEndOfFrame(); 

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            rb.AddForce(boomerangDirection * forceMultiplier);
            yield return wait;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, boomerangDirection);
    }

}
