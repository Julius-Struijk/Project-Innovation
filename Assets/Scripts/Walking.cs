using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class Walking : MonoBehaviour
{
    InputAction stepCounter;
    int stepCount;
    int prevStepCount = -1;
    public static event Action<int> OnStepCountUpdate;

    void Start()
    {
        stepCounter = InputSystem.actions.FindAction("Walking");
    }

    void Update()
    {
        stepCount = stepCounter.ReadValue<int>();
        if(prevStepCount != stepCount)
        {
            Debug.LogFormat("Step count is: {0}", stepCount);
            prevStepCount = stepCount;
            if(OnStepCountUpdate != null) { OnStepCountUpdate(stepCount); }
        }

    }
}
