using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class InputHandler : MonoBehaviour
{
    UIManager uiManager;
    public InputAction movement;
    public InputAction interact;
    public InputAction toggle;

    [SerializeField]
    int moveSensitivity = 0;

    private void Start()
    {
        if (!Accelerometer.current.enabled)
        {
            InputSystem.EnableDevice(Accelerometer.current);
        }

        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        movement = InputSystem.actions.FindAction("Movement");
        interact = InputSystem.actions.FindAction("Interact");
        toggle = InputSystem.actions.FindAction("Toggle");
    }

    private void Update()
    {
        gameObject.transform.position += new Vector3(movement.ReadValue<Vector3>().x, 0, 0)/moveSensitivity;
    }
}
