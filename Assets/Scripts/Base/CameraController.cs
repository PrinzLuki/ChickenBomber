using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraState
{   
    Aiming,
    Shot
}
public class CameraController : Singleton<CameraController>
{
    [SerializeField] Transform defaultViewTarget;
    [SerializeField] Transform viewTarget;
    [SerializeField] float maxSpeed;
    [SerializeField] float smoothTime;
    [SerializeField] float aimingFov;
    [SerializeField] float defaultFov;

    Camera mainCamera;
    CameraState currentState;
    float defaultZPos;

    void Start()
    {
        currentState = CameraState.Shot;
        mainCamera = Camera.main;
        defaultZPos = transform.position.z;
    }
    void Update()
    {
        MoveToViewTargetPosition();
        AdjustFov();
    }
    public void SetCameraState(CameraState newState)
    {
        currentState = newState;
    }
    public void SetViewTarget(Transform newViewTarget)
    {
        viewTarget = newViewTarget;
    }
    void MoveToViewTargetPosition()
    {
        if (viewTarget != null)
        {
            var cameraVelocity = mainCamera.velocity;
            var viewTargetPos = new Vector3(viewTarget.position.x, viewTarget.position.y, defaultZPos);
            mainCamera.transform.position = Vector3.SmoothDamp(mainCamera.transform.position, viewTargetPos, ref cameraVelocity, smoothTime, maxSpeed);
        }
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
}
