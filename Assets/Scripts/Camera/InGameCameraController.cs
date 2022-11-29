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
    [SerializeField] protected float smoothTime;
    [SerializeField] float yOffset;
    [SerializeField] float sideViewOffset;
    [SerializeField] protected float aimingFov;
    [SerializeField] protected float defaultFov;
    [Header("CameraShakeSettings")]
    [SerializeField] AnimationCurve shakeStrengthCurve;
    [SerializeField] float shakeSpeed;
    [SerializeField] float cameraShakeDuration;
    [SerializeField] float shakeRange;

    CameraState currentState;
    float defaultZPos;
    bool canfollow = true;

    protected override void Start()
    {
        base.Start();
        cameraBounds.center = boundingsCenter.position;
        defaultZPos = transform.position.z;
        currentState = CameraState.Shot;
    }
    protected override void Update()
    {
        if (!canfollow) return;
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
    public void CameraShake()
    {
        StartCoroutine(ShakeCamera( cameraShakeDuration));
    }
    IEnumerator ShakeCamera(float duration)
    {
        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
        Vector3 startPos = transform.position;
        canfollow = false;
        float t = 0;

        while (t < duration)
        {
            t += Time.deltaTime;
            float shakeStrength = shakeStrengthCurve.Evaluate(t/duration);

            Vector2 randomPos = Random.insideUnitCircle;
            Vector3 shakePosition = startPos + new Vector3(randomPos.x, randomPos.y, 0) * shakeStrength * shakeRange;

            transform.position = shakePosition;

            yield return waitForEndOfFrame;
        }

        canfollow = true;
    }
    void FollowTarget(Transform viewTarget)
    {
        var cameraVelocity = mainCamera.velocity;

        var viewTargetPos = new Vector3(viewTarget.position.x + sideViewOffset, viewTarget.position.y + yOffset, defaultZPos);
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
