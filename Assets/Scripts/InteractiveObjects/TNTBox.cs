using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TNTBox : DamageableEntity
{
    [SerializeField] LayerMask dmgableLayer;
    [SerializeField] float explosionRadius;
    [SerializeField] float forceMultiplier;

    protected override void Start()
    {
        OnDestroyObject += Explode;
        base.Start();
       
    }
    void Explode(int points,Vector3 _,GameObject goThis)
    {
        if (goThis != gameObject) return;
        var cam = (InGameCameraController)CameraController.Instance;
        cam.CameraShake();
        AddForceToDmgables(GetRigidBodiesInRange());
        OnDestroyObject -= Explode;
    }
    void AddForceToDmgables(List<Rigidbody> allRbsInRange)
    {
        foreach (var rb in allRbsInRange)
        {
            rb.AddForceAtPosition(GetDirectionToOtherRigidBody(rb.position) * forceMultiplier,rb.position,ForceMode.Impulse);
        }
    }

    Vector3 GetDirectionToOtherRigidBody(Vector3 otherObjPosition)
    {
        return (otherObjPosition - transform.position).normalized;
    }
    List<Rigidbody> GetRigidBodiesInRange()
    {
        var rbs = new List<Rigidbody>();
        var objsInRange = Physics.OverlapSphere(transform.position, explosionRadius,dmgableLayer);

        foreach (var collider in objsInRange)
        {
            if (collider.TryGetComponent<Rigidbody>(out Rigidbody dmgable))
            {
               rbs.Add(dmgable);
            }
        }

        return rbs;
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position,explosionRadius);
    }
}
