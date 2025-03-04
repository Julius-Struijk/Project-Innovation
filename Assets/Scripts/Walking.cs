using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System;

public class Walking : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI DEBUGTEXT, counterTMP, xpTMP;
    long stepOffset;
    bool permissionGranted = false;

    [SerializeField] List<int> xpPerStage;
    int totalXP;
    int sessionXP;
    public static event Action OnReachStage;

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
            sessionXP = ((int)stepsTaken);
            xpTMP.text = "XP: " + (totalXP + sessionXP);
        }

        // The lowest stage is always the next one, so if that one has been reached, it means a new stage has been reached.
        if(OnReachStage != null && xpPerStage != null && xpPerStage[0] < totalXP + sessionXP)
        {
            xpPerStage.RemoveAt(0);
            OnReachStage();
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
            // Once the app pauses the stepCounter gets reset to 0 so the session XP is moved to the total XP pool.
            totalXP += sessionXP;
            // Reinitialize the step counter when the app is resumed
            InitializeStepCounter();
        }
    }
}
