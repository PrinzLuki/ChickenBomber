using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OverWorldCameraController : CameraController
{
    [SerializeField] float zOffset;
    [SerializeField] float maxVelocity;
    [SerializeField] float breakForce;
    [SerializeField] Transform cameraBoundsCenter;
    [SerializeField] Bounds cameraBounds;

    Rigidbody cameraRb;
    Vector3 startDragPos;
    Vector3 endDragPos;
    Vector3 offsetVector;
    protected override void Start()
    {
        cameraBounds.center = cameraBoundsCenter.position;
        base.Start();
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
            startDragPos = mainCamera.ScreenToWorldPoint(mousePosScreen);

        }
        else if (InputManager.Instance.MouseButtonUp())
        {
            Vector3 mousePosScreen = Input.mousePosition;
            endDragPos = mainCamera.ScreenToWorldPoint(mousePosScreen);
        }
    }

    Vector3 CalculateVelocity(Vector3 startPos,Vector3 endPos)
    {
        return
    }
}
