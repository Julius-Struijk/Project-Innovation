using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwitchRooms : MonoBehaviour
{
    InputSystem_Actions inputs;
    Vector2 swipeDirection;
    [SerializeField] float minimumSwipeDistance = 10f;
    // Maximum amount of vertical deviation on the swipe.
    [SerializeField] float verticalSwipeLimit = 20f;

    // Start is called before the first frame update
    void Start()
    {
        inputs = new InputSystem_Actions();
        inputs.UI.Enable();
        inputs.UI.Tap.canceled += ProcessSwipe;
        inputs.UI.Swipe.performed += GetSwipeDirection;
    }

    void GetSwipeDirection(InputAction.CallbackContext context)
    {
        swipeDirection = context.ReadValue<Vector2>();
    }

    void ProcessSwipe(InputAction.CallbackContext context)
    {
        // If there is no swipe input or it is too small, it doesn't have to be processed.
        // Only counting swipes in the X direction, so if there is too much vertical change in the swipe it is invalidated.
        if(Mathf.Abs(swipeDirection.x) > minimumSwipeDistance && Mathf.Abs(swipeDirection.y) < verticalSwipeLimit)
        {
            Debug.LogFormat("Swiped {0} on the X-axis. with {1} on the Y-axis", Mathf.Abs(swipeDirection.x), Mathf.Abs(swipeDirection.y));
            if(swipeDirection.x > 0) { Debug.Log("Swipe Right"); }
            if (swipeDirection.x < 0) { Debug.Log("Swipe Left"); }
        }
    }
}
