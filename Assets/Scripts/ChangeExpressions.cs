using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChangeExpressions : MonoBehaviour
{
    //Reference to UI Manager
    UIManager uiManager;
    //Reference to the renderer
    [SerializeField]
    Renderer meshRenderer;
    //The expression material references
    [SerializeField]
    Material happy, neutral, hungry, tired, unhealthy, sad, neglected;
    //Maximum value of all needs combined
    float maxNeedsValue;

    [SerializeField]
    string currentMaterial;

    //Audio
    [SerializeField]
    private ZibbsNoisesScript zibbsNoisesScript;

    private float voiceTimer = 0;
    [SerializeField]
    private float minTime = 4f;
    [SerializeField]
    private float maxTime = 10f;
 
    void Start()
    {
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();

        //Set the maximum needs value
        maxNeedsValue = uiManager.maxEnergy + uiManager.maxHappiness + uiManager.maxHealth + uiManager.maxHunger;

        //Audio
        voiceTimer = Random.Range(minTime, maxTime);
    }

    //Threshold for happy face, return true if collective needs are above 80% of maximum needs value
    bool IsHappy()
    {
        if (GetNeedsValue() > maxNeedsValue * 0.8f)
        {
            return true;
        }
        else return false;
    }
    

    //Bools that say whether or not a threshold for a negative expression is met
    bool IsHungry()
    {
        if (uiManager.hungerValue < uiManager.maxHunger / 3)
        {
            return true;
        }
        else return false;
    }

    bool IsTired()
    {
        if (uiManager.energyValue < uiManager.maxEnergy / 3)
        { 
            return true;
        }
        else return false;
    }

    bool IsUnhealthy()
    {
        if (uiManager.healthValue < uiManager.maxHealth / 3)
        {
            return true;
        }
        else return false;
    }

    bool IsSad()
    {
        if (uiManager.happinessValue < uiManager.maxHappiness / 3)
        {
            return true;
        }
        else return false;

    }

    //Bool that is true if all negative thresholds have been met
    bool AllNegativesApplied()
    {
        if (IsSad() && IsHungry() && IsTired() && IsUnhealthy())
        {
            return true;
        }
        else return false;
    }

    //Bool that is true if one or more negative thresholds are met
    bool NegativeApplied()
    {
        if (IsSad() || IsTired() || IsUnhealthy() || IsHungry())
        {
            return true;
        }
        else return false;
    }

    //Gets the lowest value out of the needs
    float GetLowestValue()
    {
        float lowestValue = 0;

        if (uiManager.hungerValue <= uiManager.energyValue) { lowestValue = uiManager.hungerValue; }
        if (uiManager.happinessValue <= lowestValue) { lowestValue = uiManager.happinessValue; }
        if (uiManager.healthValue <= lowestValue) { lowestValue = uiManager.healthValue; }

        return lowestValue;
    }

    //Gets the current collective needs value
    float GetNeedsValue()
    {
        return uiManager.energyValue + uiManager.happinessValue + uiManager.healthValue + uiManager.hungerValue;
    }

    //Changes the expression depending on which need is the lowest
    void ChangeFaceToLowestValue()
    {
        float lowestValue = GetLowestValue();

        if (lowestValue == uiManager.hungerValue)
        {
            meshRenderer.material = hungry;
            currentMaterial = "hungry";
        }

        if (lowestValue == uiManager.happinessValue)
        {
            meshRenderer.material = sad;
            currentMaterial = "sad";
        }

        if (lowestValue == uiManager.healthValue)
        {
            meshRenderer.material = unhealthy;
            currentMaterial = "unhealthy";
        }

        if (lowestValue == uiManager.energyValue)
        {
            meshRenderer.material = tired;
            currentMaterial = "tired";
        }
    }

    private void Update()
    {
        //Random Timer for Audio
        voiceTimer -= Time.deltaTime;

        if (voiceTimer <= 0)
        {
            if (IsHappy())
            {
                voiceTimer = Random.Range(minTime, maxTime);
                zibbsNoisesScript.ZibbsHappyNoise();
                Debug.Log("timer has reset high");
            }
            if (!IsHappy() && !NegativeApplied())
            {
                voiceTimer = Random.Range(minTime, maxTime);
                zibbsNoisesScript.ZibbsNeutralNoise();
                Debug.Log("timer has reset mid");
            }
            if (NegativeApplied() || AllNegativesApplied())
            {
                voiceTimer = Random.Range(minTime, maxTime);
                zibbsNoisesScript.ZibbsSadNoise();
                Debug.Log("timer has reset low");
            }
        }
        
        //If the threshold to one or more negative expressions are met, change the face to the lowest one
        if (NegativeApplied() && !AllNegativesApplied() && currentMaterial != "hungry" && currentMaterial != "sad" && currentMaterial != "unhealthy" && currentMaterial != "tired")
        {
            ChangeFaceToLowestValue();
            zibbsNoisesScript.ZibbsHiMiNoise();
            Debug.Log("i'm a bitch");
        }

        //If all negatives are active, make the face neglected
        if (AllNegativesApplied() && currentMaterial != "neglected")
        {
            meshRenderer.material = neglected;
            currentMaterial = "neglected";
            zibbsNoisesScript.ZibbsMiLoNoise();
        }

        //If the happy threshold is met, the expression turns happy
        if (IsHappy() && currentMaterial != "happy")
        {
            meshRenderer.material = happy;
            currentMaterial = "happy";
            zibbsNoisesScript.ZibbsMiHiNoise();
        }

        //If no thresholds are met, the face is neutral
        if (!NegativeApplied() && !IsHappy() && currentMaterial != "neutral")
        {
            Debug.Log("I am NOT a bitch");
            meshRenderer.material = neutral;
            currentMaterial = "neutral";
            zibbsNoisesScript.ZibbsLoMiNoise();
        }
    }
}
