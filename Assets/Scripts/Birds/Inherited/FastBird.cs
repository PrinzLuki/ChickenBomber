using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastBird : BaseBird
{
    [Header("FastBird Settings")]
    [SerializeField] float boostSpeed;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            UseAbility();
        }
    }


    protected override void UseAbility()
    {
        var direction = rb.velocity;

        rb.AddForce(direction * boostSpeed);

        Debug.Log("UsedBoost");
    }

    protected override void OnCollisionEnter(Collision other)
    {
        base.OnCollisionEnter(other);
    }
}
