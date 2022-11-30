using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Fan : MonoBehaviour
{
    [SerializeField] Transform fanImpactZoneOrigin;
    [SerializeField] LayerMask affectedLayer;
    [SerializeField] Vector3 fanImpactSize;
    [SerializeField] float fanForceMultiplier;

    void FixedUpdate()
    {
        ApplyForceToRigidBodies(GetAllRigidBodiesInFanZone());
    }

    void ApplyForceToRigidBodies(List<Rigidbody> rbs)
    {
        for (int i = 0; i < rbs.Count; i++)
        {
            rbs[i].AddForce(transform.forward * fanForceMultiplier,ForceMode.Force);
        }
    }
    List<Rigidbody> GetAllRigidBodiesInFanZone()
    {
        var objectsInZone = Physics.OverlapBox(fanImpactZoneOrigin.position, fanImpactSize, fanImpactZoneOrigin.rotation, affectedLayer);
        var rigidBodies = new List<Rigidbody>();

        foreach (var obj in objectsInZone)
        {
            if (obj.TryGetComponent(out Rigidbody rb))
            {
                rigidBodies.Add(rb);
            }
        }
        return rigidBodies;
    }
    void OnDrawGizmos()
    {
        if (fanImpactZoneOrigin != null)
        {
            Gizmos.DrawWireCube(fanImpactZoneOrigin.position, fanImpactSize);
        }
    }
}
