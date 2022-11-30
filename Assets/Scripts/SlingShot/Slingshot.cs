using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SlingshotState
{
    None,
    Loaded,
    Shot
}
public class Slingshot : MonoBehaviour
{
    [Header("SlingShotSettings")]
    [SerializeField] Transform launchPoint;
    [SerializeField] float minPower;
    [SerializeField] float maxPower;
    [SerializeField] float lineSpawnOffset;
    [SerializeField] float offset;
    [SerializeField] float powerMultiplier;
    [SerializeField] float reloadTime;

    TrajectoryLine trajectoryLine;
    InGameCameraController ingameCameraController;
    Camera cam;
    Rigidbody currentBirdRb;
    Vector2 startPosition;
    Vector2 currentPosition;
    SlingshotState currentState = SlingshotState.None;

    void Awake()
    {
        LevelManager.OnReload += StartReloadingSlingShot;
    }
    void Start()
    {
        ingameCameraController = (InGameCameraController)CameraController.Instance;
        trajectoryLine = GetComponent<TrajectoryLine>();
        currentState = SlingshotState.None;
        cam = Camera.main;
        offset = Mathf.Abs( transform.position.z - cam.nearClipPlane);
    }

    void OnDestroy()
    {
        LevelManager.OnReload -= StartReloadingSlingShot;
    }
    void Update()
    {
        if (InputManager.Instance.MouseButtonDown() && currentState == SlingshotState.Loaded)
        {
            ingameCameraController.SetCameraState(CameraState.Aiming);
            SetStartPos(GetMousePos());
            return;
        }
        else if (InputManager.Instance.IsDragging() && currentState == SlingshotState.Loaded) 
        {
            SetCurrentPos(GetMousePos());
            trajectoryLine.SetTrajectoryLineActive(true);
            trajectoryLine.SetLineRenderPositions(currentBirdRb,transform.position + new Vector3(0,lineSpawnOffset,0),CalculateVelocity());
            return;
        }
        else if (InputManager.Instance.MouseButtonUp() && currentState == SlingshotState.Loaded && CalculateVelocity().magnitude > minPower)
        {
            var bird = currentBirdRb.GetComponent<BaseBird>();
            ingameCameraController.SetCameraState(CameraState.Shot);
            PlayBirdSound(bird, SoundFileType.Launched);
            SetSlingShotShot();
            LaunchBird(bird);
        }
        else if (InputManager.Instance.MouseButtonUp() && currentState == SlingshotState.Loaded &&
                 CalculateVelocity().magnitude < minPower)
        {
           ingameCameraController.SetCameraState(CameraState.Shot);
            trajectoryLine.SetTrajectoryLineActive(false);
        }
    }
    void PlayBirdSound(BaseBird bird,SoundFileType soundFileType)
    {
        bird.TryGetComponent(out SoundManagmentComponent soundComp);
        var instance = (InGameSoundManager)SoundManager.Instance;
        instance.SpawnSoundObject(soundComp.GetAudioClipFromSoundFileOfType(soundFileType),bird.transform.position,AudioMixerGroupType.Sfx);
    }
    void SetSlingShotShot()
    {
        trajectoryLine.SetTrajectoryLineActive(false);
        currentState = SlingshotState.Shot;
    }
    void LaunchBird(BaseBird bird)
    {
        bird.SetisLaunched(true);
        currentBirdRb.AddForce(CalculateVelocity(), ForceMode.Impulse);
    }
    void StartReloadingSlingShot(Rigidbody bird)
    {
        currentBirdRb = bird;
        CameraController.Instance.SetViewTarget(bird.transform);
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

        currentState = SlingshotState.Loaded;
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
