using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class InputHandler : MonoBehaviour
{
    UIManager uiManager;
    InputAction movement;
    public InputAction interact;

    [SerializeField]
    int moveSensitivity = 0;

    private void Start()
    {
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        movement = InputSystem.actions.FindAction("Movement");
        interact = InputSystem.actions.FindAction("Interact");
    }

    private void Update()
    {
        gameObject.transform.position += new Vector3(movement.ReadValue<Vector3>().x, 0, 0)/moveSensitivity;
    }
}
