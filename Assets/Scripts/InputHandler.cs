using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Android;

public class InputHandler : MonoBehaviour
{
    //Reference to the camera
    public Camera cam;

    //Input actions to define
    public InputAction movement;
    public InputAction interact;
    public InputAction toggle;

    private void Start()
    {
        cam = Camera.main;

        //Make sure the android devices are enabled
        if (AndroidGyroscope.current != null && !AndroidGyroscope.current.enabled)
        {
            InputSystem.EnableDevice(AndroidGyroscope.current);
        }

        if (AndroidAccelerometer.current != null && !AndroidAccelerometer.current.enabled)
        {
            InputSystem.EnableDevice(AndroidAccelerometer.current);
        }

        //Connecting the Input to the correct actions
        movement = InputSystem.actions.FindAction("Movement");
        interact = InputSystem.actions.FindAction("Interact");
        toggle = InputSystem.actions.FindAction("Toggle");
    }

    public float ScreenMaximumX()
    {
        Vector3 topRight = cam.ViewportToWorldPoint(new Vector3(1, 1, 0));
        return topRight.x;
    }

    public float ScreenMinimumX()
    {
        Vector3 bottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, 0));
        return bottomLeft.x;
    }

    public float ScreenMaximumY()
    {
        Vector3 topRight = cam.ViewportToWorldPoint(new Vector3(1, 1, 0));
        return topRight.y;
    }

    public float ScreenMinimumY()
    {
        Vector3 bottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, 0));
        return bottomLeft.y;
    }

    public Vector2 GetObjectScreenPos(GameObject obj)
    {
        return cam.WorldToScreenPoint(obj.transform.position);
    }

    public Vector2 GetScreenToWorldPos(Vector2 screenPos)
    {
        return cam.ScreenToWorldPoint(screenPos);
    }

    public float GetCameraWidth()
    {
        return cam.pixelWidth;
    }
}
