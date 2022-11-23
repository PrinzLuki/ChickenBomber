using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    [SerializeField] Transform defaultViewTarget;
    [SerializeField] Transform viewTarget;
    [SerializeField] float maxSpeed;
    [SerializeField] float smoothTime;
    [SerializeField] float aimingFov;
    [SerializeField] float defaultFov;

    Camera mainCamera;
    float defaultZPos;

    void Start()
    {
        mainCamera = Camera.main;
        defaultZPos = transform.position.z;
        viewTarget = defaultViewTarget;
    }
    void Update()
    {
        MoveToViewTargetPosition();
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
}
