using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdHeavy : BaseBird
{
    [Header("Heavy Bird Settings")]
    [SerializeField, Range(1, 500)] float mass;
    [SerializeField, Range(1, 1000)] float forceToPushDown;
    [SerializeField, Range(0.5f, 20f)] float forceTime = 3;

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

        //getting really heavy
        rb.mass = mass;
        StartCoroutine(OnAbilityTimeSpan());
    }

    IEnumerator OnAbilityTimeSpan()
    {
        float timer = forceTime;
        while (timer > 0)
        {
            timer -= Time.deltaTime;

            rb.AddForce(Vector3.down * forceToPushDown);
            yield return new WaitForEndOfFrame();
        }
    }

    protected override void OnCollisionEnter(Collision other)
    {
        base.OnCollisionEnter(other);
        DisableAbility();
    }
}
