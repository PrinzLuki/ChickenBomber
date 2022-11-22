using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastBird : BaseBird
{
    protected override void UseAbility()
    {
        
    }

    protected override void OnCollisionEnter(Collision other)
    {
        base.OnCollisionEnter(other);
    }
}
