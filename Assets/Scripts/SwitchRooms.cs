using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class SwitchRooms : MonoBehaviour
{

    InputSystem_Actions inputs;
    Vector2 swipeDirection;
    [SerializeField] float minimumSwipeDistance = 10f;
    // Maximum amount of vertical deviation on the swipe.
    [SerializeField] float verticalSwipeLimit = 20f;

    [SerializeField] List<GameObject> screens;
    // This is the screen the project, technically always starts on, unless the position of the screens has been moved in the editor.
    int currentScreenIndex = 1;
    [SerializeField] int startScreenIndex = 1;
    // How long it will take for the room to switch in milliseconds, currently unused.
    //float switchTime = 300;
    static float _screenWidth;

    // Start is called before the first frame update
    void Start()
    {
        inputs = new InputSystem_Actions();
        inputs.UI.Enable();
        inputs.UI.Tap.canceled += ProcessSwipe;
        inputs.UI.Swipe.performed += GetSwipeDirection;

        //Get screen width so that screens can be offset properly while switching.
        RectTransform screenRect = gameObject.transform.parent.GetComponent<RectTransform>();

        _screenWidth = screenRect.rect.width;
        _screenWidth *= screenRect.lossyScale.x;
        Debug.LogFormat("Screen width set to: {0}", _screenWidth);

        //Disable non-start screens.
        for (int i = 0; i < screens.Count; i++)
        {
            if (i != currentScreenIndex)
            {
                screens[i].SetActive(false);
            }
        }

        //Moving the start screen into the canvas.
        RoomSwitch(startScreenIndex - currentScreenIndex);
    }

    void GetSwipeDirection(InputAction.CallbackContext context)
    {
        swipeDirection = context.ReadValue<Vector2>();
    }

    void ProcessSwipe(InputAction.CallbackContext context)
    {
        // If there is no swipe input or it is too small, it doesn't have to be processed.
        // Only counting swipes in the X direction, so if there is too much vertical change in the swipe it is invalidated.
        if (Mathf.Abs(swipeDirection.x) > minimumSwipeDistance && Mathf.Abs(swipeDirection.y) < verticalSwipeLimit)
        {
            //Debug.LogFormat("Swiped {0} on the X-axis. with {1} on the Y-axis", Mathf.Abs(swipeDirection.x), Mathf.Abs(swipeDirection.y));
            if (swipeDirection.x > 0)
            {
                RoomSwitch(-1);
            }
            if (swipeDirection.x < 0)
            {
                RoomSwitch(1);
            }
        }
    }

    //Allows switching from any room to any other room.
    void RoomSwitch(int changeAmount)
    {
        Debug.LogFormat("Room switch change amount is {0}", changeAmount);
        if(changeAmount < 0 && (currentScreenIndex + changeAmount) >= 0)
        {
            Debug.Log("Swipe Right, moving left.");
            Debug.LogFormat("Old screen position: {0}",  transform.position);
            transform.position += new Vector3(_screenWidth * Mathf.Abs(changeAmount), 0, 0);
            Debug.LogFormat("New screen position: {0}", transform.position);
            screens[currentScreenIndex].SetActive(false);
            currentScreenIndex += changeAmount;
            screens[currentScreenIndex].SetActive(true);


        }
        else if(changeAmount > 0 && (currentScreenIndex + changeAmount) <= (screens.Count - 1))
        {
            Debug.Log("Swipe Left, moving right.");
            Debug.LogFormat("Old screen position: {0}", transform.position);
            transform.position -= new Vector3(_screenWidth * Mathf.Abs(changeAmount), 0, 0);
            Debug.LogFormat("New screen position: {0}", transform.position);
            screens[currentScreenIndex].SetActive(false);
            currentScreenIndex += changeAmount;
            screens[currentScreenIndex].SetActive(true);
        }
    }
}
