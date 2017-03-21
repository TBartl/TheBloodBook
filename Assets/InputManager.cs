using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager {

    public static bool IsHeld() {
        return (Input.touchCount != 0 || Input.GetMouseButton(0));
    }

    public static Vector3 GetPosition() {
        if (!IsHeld())
            return Vector3.zero;
        if (Input.touchCount > 0)
            return Input.GetTouch(0).position;
        else
            return Input.mousePosition;


    }

}
