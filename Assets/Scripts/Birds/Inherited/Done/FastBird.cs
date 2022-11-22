using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastBird : BaseBird
{
    [Header("FastBird Settings")]
    [SerializeField, Range(0, 1000)] float boostSpeed;
    [SerializeField, Range(0, 10)] float abilityTimeSpan;
    [SerializeField] Vector3 directionOffset;

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
        StartCoroutine(OnAbilityTimeSpan());
        Debug.Log("UsedBoost");

    }

    IEnumerator OnAbilityTimeSpan()
    {
        float timer = abilityTimeSpan;
        while(timer > 0)
        {
            timer -= Time.deltaTime;
            rb.AddForce(directionOffset * boostSpeed, ForceMode.Impulse);

            yield return new WaitForEndOfFrame();
        }
    }

    protected override void OnCollisionEnter(Collision other)
    {
        base.OnCollisionEnter(other);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, directionOffset);
    }
}
