using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    [SerializeField] GameObject bird;
    [Header("SlingShotSettings")]
    [SerializeField] Vector2 startPosition;
    [SerializeField] Vector2 currentPosition;

    void Start()
    {
        Camera cam = Camera.main;
    }
    void Update()
    {
        if (InputManager.Instance.MouseButtonDown())
        {
            SetStartPos(InputManager.Instance.GetMousePos());
            return;
        }
        else if (InputManager.Instance.IsDragging())
        {
            SetCurrentPos(InputManager.Instance.GetMousePos());
            return;
        }
    }
    void SetStartPos(Vector2 touchStartPos)
    {
        startPosition = touchStartPos;
    }
    void SetCurrentPos(Vector2 currentTouchPos)
    {
        currentPosition = currentTouchPos;
        bird.transform.position = currentPosition;
    }
}
