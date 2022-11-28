using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class CameraController : Singleton<CameraController>
{
    [SerializeField] protected float maxSpeed;
    
    protected Camera mainCamera;
    
    protected virtual void Start()
    {
        mainCamera = Camera.main;
    }
    protected virtual void Update()
    {
        MoveToViewTargetPosition();
    }
    public abstract void SetViewTarget(Transform newViewTarget);
    protected abstract void MoveToViewTargetPosition();
    
}
