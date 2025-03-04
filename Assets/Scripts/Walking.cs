using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System;

public class Walking : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI DEBUGTEXT, counterTMP;
    long stepOffset;
    bool permissionGranted = false;

    void Start()
    {
        if (Application.isEditor)
        {
            DEBUGTEXT.text = "Running in Editor";
            return;
        }

        RequestPermission();
    }

    void Update()
    {
        if (Application.isEditor || !permissionGranted)
        {
            return;
        }

        if (StepCounter.current == null)
        {
            DEBUGTEXT.text = "StepCounter not available";
            return;
        }

        if (stepOffset == 0)
        {
            stepOffset = StepCounter.current.stepCounter.ReadValue();
            DEBUGTEXT.text = "Step offset " + stepOffset;
        }
        else
        {
            long currentSteps = StepCounter.current.stepCounter.ReadValue();
            long stepsTaken = currentSteps - stepOffset;
            counterTMP.text = "Steps: " + stepsTaken;
        }
    }

    async void RequestPermission()
    {
#if UNITY_EDITOR
        DEBUGTEXT.text = "Editor Platform";
#endif
#if UNITY_ANDROID
        AndroidRuntimePermissions.Permission result = await AndroidRuntimePermissions.RequestPermissionAsync("android.permission.ACTIVITY_RECOGNITION");
        if (result == AndroidRuntimePermissions.Permission.Granted)
        {
            permissionGranted = true;
            DEBUGTEXT.text = "Permission granted";
            InitializeStepCounter();
        }
        else
        {
            DEBUGTEXT.text = "Permission state: " + result;
        }
#endif
    }

    void InitializeStepCounter()
    {
        InputSystem.EnableDevice(StepCounter.current);
        stepOffset = StepCounter.current.stepCounter.ReadValue();
    }

    void OnApplicationPause(bool pause)
    {
        if (!pause && permissionGranted)
        {
            // Reinitialize the step counter when the app is resumed
            InitializeStepCounter();
        }
    }
}
