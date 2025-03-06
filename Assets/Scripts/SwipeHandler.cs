using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwipeHandler : MonoBehaviour
{
    public float swipeThreshold;
    public float swipeCooldown;

    public InputAction swipe;
    bool swiping = false;

    Vector2 swipeStartPos;
    Vector2 swipeEndPos;
    Vector2 swipeDelta;

    void Start()
    {
        
    }

    private void OnEnable()
    {
        swipe.Enable();

        swipe.performed += PerformingSwipe;
        swipe.canceled += SwipeEnd;
    }

    void PerformingSwipe(InputAction.CallbackContext context)
    {
        Vector2 delta = context.ReadValue<Vector2>();

        swipeDelta = delta;

        if (swipeDelta.magnitude >= swipeThreshold && !swiping)
        {
            swiping = true;
            Debug.Log("Swipe");
        }
    }

    void SwipeEnd(InputAction.CallbackContext context)
    {
    }

    private void OnDisable()
    {
        swipe.performed -= PerformingSwipe;
        swipe.canceled -= SwipeEnd;
    }

    void Update()
    {
        
    }
}
