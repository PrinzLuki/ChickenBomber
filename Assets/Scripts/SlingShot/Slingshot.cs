using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SlingshotState
{
    Loaded,
    Shot
}
public class Slingshot : MonoBehaviour
{
    [SerializeField] Rigidbody bird;
    [Header("SlingShotSettings")]
    [SerializeField] Transform launchPoint;
    [SerializeField] float maxPower;
    [SerializeField] float lineSpawnOffset;
    [SerializeField] float offset;
    [SerializeField] float powerMultiplier;
    [SerializeField] float reloadTime;
    Vector2 startPosition;
    Vector2 currentPosition;
    [SerializeField] SlingshotState currentState;

    TrajectoryLine trajectoryLine;
    Camera cam;

    void Start()
    {
        trajectoryLine = GetComponent<TrajectoryLine>();

        cam = Camera.main;
        offset = transform.position.z - cam.nearClipPlane;
        bird.useGravity = false;
        StartCoroutine(ReloadSlingShot(bird.transform));
    }
    void Update()
    {
        if (InputManager.Instance.MouseButtonDown() && currentState == SlingshotState.Loaded)
        {
            SetStartPos(GetMousePos());
            return;
        }
        else if (InputManager.Instance.IsDragging() && currentState == SlingshotState.Loaded) 
        {
            SetCurrentPos(GetMousePos());
            trajectoryLine.SetTrajectoryLineActive(true);
            trajectoryLine.SetLineRenderPositions(bird,transform.position + new Vector3(0,lineSpawnOffset,0),CalculateVelocity());
            return;
        }
        else if (InputManager.Instance.MouseButtonUp() && currentState == SlingshotState.Loaded)
        {
            trajectoryLine.SetTrajectoryLineActive(false);
            currentState = SlingshotState.Shot;
            bird.useGravity = true;
            bird.AddForce(CalculateVelocity(),ForceMode.Impulse);
        }
    }

    void StartReloadingSlingShot(Rigidbody bird)
    {
        StartCoroutine(ReloadSlingShot(bird.transform));
    }
    Vector2 GetMousePos()
    {
        Vector2 mousePos = Input.mousePosition;
        Vector3 worldPoint = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane + offset));
      
        return new Vector2(worldPoint.x,worldPoint.y);
    }
    Vector2 CalculateVelocity()
    {
        Vector2 velocity = (startPosition - currentPosition) * powerMultiplier;
        velocity = Vector2.ClampMagnitude(velocity, maxPower);
        return velocity;
    }

    IEnumerator ReloadSlingShot(Transform birdTransform)
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();
        float t = 0;
        while (t < reloadTime)
        {
            t += Time.deltaTime;
            birdTransform.position = Vector3.Slerp(birdTransform.position, launchPoint.position, t / reloadTime);
            yield return wait;
        }
    }
    void SetStartPos(Vector2 touchStartPos)
    {
        startPosition = touchStartPos;
    }
    void SetCurrentPos(Vector2 currentTouchPos)
    {
        currentPosition = currentTouchPos;
    }
}
