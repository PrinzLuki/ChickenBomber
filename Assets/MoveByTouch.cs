using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveByTouch : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Camera cam = Camera.main;
            Vector2 mousePos = Vector3.zero;
            mousePos = Input.mousePosition;

            Vector3 worldPoint = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane));

            transform.position = worldPoint;
        }
    }
}
