using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InputManager :SingletonL<InputManager>
{
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
