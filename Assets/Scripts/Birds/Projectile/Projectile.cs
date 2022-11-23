using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField] float forwardForce;
    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        ProjectileForce();
    }

    void ProjectileForce()
    {
        rb.AddForce(transform.forward * forwardForce, ForceMode.Impulse);
    }
}
