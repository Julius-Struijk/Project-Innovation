using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputTest : MonoBehaviour
{
    InputAction interactAction;

    void Start()
    {
        interactAction = InputSystem.actions.FindAction("Interact");
    }

    void Update()
    {
        
    }
}
