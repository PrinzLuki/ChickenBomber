using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraState
{
    Aiming,
    Shot
}
public class InGameCameraController : CameraController
{
    // maybe think about changing the position of the camera and not the fov because it stretches everything
    [Header("CameraSettings")]
    [SerializeField] Bounds cameraBounds;
    [SerializeField] Transform boundingsCenter;
    [SerializeField] protected Transform defaultViewTarget;
    [SerializeField] protected Transform viewTarget;
    [SerializeField] float sideViewOffset;
    [SerializeField] protected float aimingFov;
    [SerializeField] protected float defaultFov;

    CameraState currentState;
    float defaultZPos;

    protected override void Start()
    {
        base.Start();
        cameraBounds.center = boundingsCenter.position;
        defaultZPos = transform.position.z;
        currentState = CameraState.Shot;
    }
    protected override void Update()
    {
        base.Update();
        AdjustFov();
    }
    public void SetCameraState(CameraState newState)
    {
        currentState = newState;
    }
    protected override void MoveToViewTargetPosition()
    {
        if (viewTarget != null && cameraBounds.Contains(viewTarget.position))
        {
          FollowTarget(viewTarget);
        }
        else
        {
            viewTarget = defaultViewTarget;
            FollowTarget(viewTarget);
        }
    }

    void FollowTarget(Transform viewTarget)
    {
        var cameraVelocity = mainCamera.velocity;

        var viewTargetPos = new Vector3(viewTarget.position.x + sideViewOffset, viewTarget.position.y, defaultZPos);
        mainCamera.transform.position = Vector3.SmoothDamp(mainCamera.transform.position, viewTargetPos, ref cameraVelocity, smoothTime, maxSpeed);
    }
    public override void SetViewTarget(Transform newViewTarget)
    {
        viewTarget = newViewTarget;
       
    }
    void AdjustFov()
    {
        switch (currentState)
        {
            case CameraState.Aiming:
                mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, aimingFov, Time.deltaTime);
                break;
            case CameraState.Shot:
                mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, defaultFov, Time.deltaTime);
                break;
        }
    }

    void OnDrawGizmos()
    {
        if (boundingsCenter != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(boundingsCenter.position,cameraBounds.size);
        }
    }
}
