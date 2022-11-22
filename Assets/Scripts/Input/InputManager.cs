using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager :SingletonL<InputManager>
{
    Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
    }
    public Vector2 GetMousePos()
    {
        Vector2 mousePos = Input.mousePosition;
        Vector3 worldPoint = mainCam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, mainCam.nearClipPlane));
        return worldPoint;
    }

    public bool MouseButtonDown()
    {
        return Input.GetMouseButtonDown(0);
    }

    public bool MouseButtonUp()
    {
        return Input.GetMouseButtonUp(0);
    }
    public bool IsDragging()
    {
        return Input.GetMouseButton(0);
    }
}
