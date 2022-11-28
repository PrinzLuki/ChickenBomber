using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class OverWorldCameraController : CameraController
{
    //break force is determined by the drag of the cameras rb

    [Header("CameraSettings")]
    [SerializeField] OverWorldRuntimeSaveData runtimeSaveData;
    [SerializeField] Transform orientationPos;
    [SerializeField] Transform groundPlane;
    [SerializeField] Transform cameraBoundsCenter;
    [SerializeField] float nearPlaneOffset;
    [SerializeField] float cameraSpeedMultiplier;
    [SerializeField] Bounds cameraBounds;

    Rigidbody cameraRb;
    [SerializeField]Vector3 startDragPos;
    [SerializeField]Vector3 endDragPos;
    Vector3 offsetVector;
    protected override void Start()
    {
        base.Start();
        cameraBounds.center = cameraBoundsCenter.position;
        cameraRb = GetComponent<Rigidbody>();
        nearPlaneOffset = (transform.position.y - mainCamera.nearClipPlane) - groundPlane.position.y;
        transform.position = runtimeSaveData.lastPositionCamera;
    }

    protected override void Update()
    {
        base.Update();
        ClampCameaPosition();
        ClampVelocity();
    }

    public override void SetViewTarget(Transform newViewTarget)
    {
       Debug.Log("Empty but may come in handy for different usages");
    }
    protected override void MoveToViewTargetPosition()
    {
        if(InputManager.Instance.MouseButtonDown())
        {
            Vector3 mousePosScreen = Input.mousePosition;
            Vector3 mousePosWithNearplaneOffset = new Vector3(mousePosScreen.x, mousePosScreen.y, mainCamera.nearClipPlane + nearPlaneOffset);
            startDragPos = mainCamera.ScreenToWorldPoint(mousePosWithNearplaneOffset);
        }
        else if (InputManager.Instance.IsDragging())
        {
            Vector3 mousePosScreen = Input.mousePosition;
            Vector3 mousePosWithNearplaneOffset = new Vector3(mousePosScreen.x, mousePosScreen.y, mainCamera.nearClipPlane + nearPlaneOffset);
            endDragPos = mainCamera.ScreenToWorldPoint(mousePosWithNearplaneOffset);
            Vector3 velocity = CalculateVelocity();
            cameraRb.AddForce(new Vector3(velocity.x,0,velocity.z),ForceMode.Force);
        }
    }
    Vector3 CalculateVelocity()
    {
        Vector3 velocity;

        velocity = (endDragPos - orientationPos.position).normalized * cameraSpeedMultiplier;

        return velocity;
    }

    void ClampVelocity()
    {
        cameraRb.velocity = Vector3.ClampMagnitude(cameraRb.velocity,maxSpeed);
    }
    void ClampCameaPosition()
    {
        if (!cameraBounds.Contains(transform.position))
        {
            transform.position = cameraBounds.ClosestPoint(transform.position);
        }
    }
    void OnDrawGizmos()
    {
        if (cameraBoundsCenter != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(cameraBoundsCenter.position,cameraBounds.size);
        }
    }
}
