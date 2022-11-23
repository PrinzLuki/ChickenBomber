using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdMultipleProjectiles : BaseBird
{
    [Header("References and so stuff")]
    [SerializeField] int projectileAmount;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float speed;
    [SerializeField] Vector2 startPoint;

    float radius;


    private void Start()
    {
        radius = 5;
        speed = 5;
    }

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

        float anglestep = -90f / projectileAmount;
        float angle = 180;
        Debug.LogWarning("Activates Projectile");
        for (int i = 0; i < projectileAmount; i++)
        {
            
            float xPos = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180) *radius;
            float yPos = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180) * radius;

            Vector2 projectileVector = new Vector2(xPos, yPos);
            Vector2 projectileMoveDirection = (projectileVector - (Vector2)transform.position).normalized * speed;

            var tempProjectile = Instantiate(projectilePrefab);
            tempProjectile.transform.SetPositionAndRotation(transform.position, Quaternion.Euler(0, 90, 0));
            tempProjectile.GetComponent<Rigidbody>().velocity = projectileMoveDirection;
            tempProjectile.transform.forward = projectileMoveDirection;
            angle += anglestep;
        }
    }


    protected override void OnCollisionEnter(Collision other)
    {
        base.OnCollisionEnter(other);
        DisableAbility();
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.white;
    //    Gizmos.DrawRay(transform.position, startDirection);
    //    Gizmos.DrawRay(transform.position, endDirection);

    //    for (int i = 0; i < projectileAmount; i++)
    //    {
    //        Debug.Log("step: " + step);

    //        Gizmos.DrawRay(transform.position, startDirection + step);
    //    }
    //}
}
