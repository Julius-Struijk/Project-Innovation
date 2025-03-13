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

    float totalWalkingXP = 0f;
    float sessionXP = 0f;
    float prevGivenXP = 0f;
    XPManager xpManager;

    void Start()
    {
        if (Application.isEditor)
        {
            if (DEBUGTEXT != null)
            {
                DEBUGTEXT.text = "Running in Editor";
            }
            return;
        }

        RequestPermission();

        xpManager = GameObject.FindGameObjectWithTag("XPManager").GetComponent<XPManager>();
    }

    void Update()
    {
        if (Application.isEditor || !permissionGranted)
        {
            return;
        }

        if (StepCounter.current == null && DEBUGTEXT != null)
        {
            DEBUGTEXT.text = "StepCounter not available";
            return;
        }

        if (stepOffset == 0)
        {
            stepOffset = StepCounter.current.stepCounter.ReadValue();
            if (DEBUGTEXT != null)
            {
                DEBUGTEXT.text = "Step offset " + stepOffset;
            }
        }
        else
        {
            long currentSteps = StepCounter.current.stepCounter.ReadValue();
            long stepsTaken = currentSteps - stepOffset;
            if (counterTMP != null)
            {
                counterTMP.text = "Steps This Session: " + stepsTaken;
            }
            sessionXP = ((float)stepsTaken);
        }

        xpManager.AddXP((totalWalkingXP + sessionXP) - prevGivenXP);
        prevGivenXP = totalWalkingXP + sessionXP;
    }

    async void RequestPermission()
    {
#if UNITY_EDITOR
        if (DEBUGTEXT != null)
        {
            DEBUGTEXT.text = "Editor Platform";
        }
#endif
#if UNITY_ANDROID
        AndroidRuntimePermissions.Permission result = await AndroidRuntimePermissions.RequestPermissionAsync("android.permission.ACTIVITY_RECOGNITION");
        if (result == AndroidRuntimePermissions.Permission.Granted)
        {
            permissionGranted = true;
            if (DEBUGTEXT != null) { DEBUGTEXT.text = "Permission granted"; }
            InitializeStepCounter();
        }
        else
        {
            if (DEBUGTEXT != null) { DEBUGTEXT.text = "Permission state: " + result; }
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
            // Once the app pauses the stepCounter gets reset to 0 so the session XP is moved to the total XP pool.
            totalWalkingXP += sessionXP;
            // Reinitialize the step counter when the app is resumed
            InitializeStepCounter();
        }
    }
}
