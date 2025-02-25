using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class InputTest : MonoBehaviour
{
    InputAction movement;
    [SerializeField]
    int moveSensitivity = 0;

    private void Start()
    {
        movement = InputSystem.actions.FindAction("Movement");
    }

    private void Update()
    {
        gameObject.transform.position += new Vector3(movement.ReadValue<Vector3>().x, 0, 0)/moveSensitivity;
    }
}
