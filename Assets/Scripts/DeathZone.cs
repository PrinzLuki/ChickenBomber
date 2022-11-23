using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    [Header("Damage Settings")]
    [SerializeField] private Vector3 zoneScale;
    [SerializeField] private float damageInZone = 999f;

    private void Awake()
    {
        transform.localScale = zoneScale;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<IDamageable>() != null)
            other.gameObject.GetComponent<IDamageable>().GetDmg(other.rigidbody, damageInZone);
    }
}
