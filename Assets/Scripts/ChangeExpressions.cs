using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChangeExpressions : MonoBehaviour
{
    //Turn on changes based on expression
    [SerializeField]
    bool toggleNeedsExpressions;
    //Reference to UI Manager
    UIManager uiManager;
    //Reference to the renderer
    [SerializeField]
    Renderer meshRenderer;
    //The expression material references
    [SerializeField]
    Material happy, neutral, hungry, tired, unhealthy, sad, neglected, sleeping;
    //Maximum value of all needs combined
    float maxNeedsValue;

    bool asleep;


    void Start()
    {
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();

        //Set the maximum needs value
        maxNeedsValue = uiManager.maxEnergy + uiManager.maxHappiness + uiManager.maxHealth + uiManager.maxHunger;
    }

    public void ToggleSleeping()
    {
        if (asleep)
        {
            asleep = false;
            return;
        }
        else
        {
            asleep = true;
        }
    }

    //change expression how you want
    public void ChangeExpression(string expression)
    {
        switch (expression)
        {
            case "Sleeping":
                meshRenderer.material = sleeping;
                break;
            case "Neutral":
                meshRenderer.material = neutral;
                break;
            case "Happy":
                meshRenderer.material = happy;
                break;
            default:
                Debug.Log("No expression matches input string");
                break;
        }

    }

    //Threshold for happy face, return true if collective needs are above 80% of maximum needs value
    bool IsHappy()
    {
        if (GetNeedsValue() > maxNeedsValue * 0.8f && toggleNeedsExpressions)
        {
            return true;
        }
        else return false;
    }


    //Bools that say whether or not a threshold for a negative expression is met
    bool IsHungry()
    {
        if (uiManager.hungerValue < uiManager.maxHunger / 3 && toggleNeedsExpressions)
        {
            return true;
        }
        else return false;
    }

    bool IsTired()
    {
        if (uiManager.energyValue < uiManager.maxEnergy / 3 && toggleNeedsExpressions)
        {
            return true;
        }
        else return false;
    }

    bool IsUnhealthy()
    {
        if (uiManager.healthValue < uiManager.maxHealth / 3 && toggleNeedsExpressions)
        {
            return true;
        }
        else return false;
    }

    bool IsSad()
    {
        if (uiManager.happinessValue < uiManager.maxHappiness / 3 && toggleNeedsExpressions)
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

        if (lowestValue == uiManager.hungerValue && meshRenderer.material != hungry)
        {
            //Debug.LogFormat("Changing face from {0} to hungry.", meshRenderer.material.name);
            meshRenderer.material = hungry;
        }

        if (lowestValue == uiManager.happinessValue && meshRenderer.material != sad)
        {
            //Debug.LogFormat("Changing face from {0} to sad.", meshRenderer.material.name);
            meshRenderer.material = sad;
        }

        if (lowestValue == uiManager.healthValue && meshRenderer.material != unhealthy)
        {
            //Debug.LogFormat("Changing face from {0} to unhealthy.", meshRenderer.material.name);
            meshRenderer.material = unhealthy;
        }

        if (lowestValue == uiManager.energyValue && meshRenderer.material != tired)
        {
            //Debug.LogFormat("Changing face from {0} to tired.", meshRenderer.material.name);
            meshRenderer.material = tired;
        }
    }

    private void Update()
    {
        //If the threshold to one or more negative expressions are met, change the face to the lowest one
        if (NegativeApplied() && !asleep)
        {
            ChangeFaceToLowestValue();
        }

        //If all negatives are active, make the face neglected
        if (AllNegativesApplied() && meshRenderer.material != neglected && !asleep)
        {
            //Debug.LogFormat("Changing face from {0} to neglected.", meshRenderer.material.name);
            meshRenderer.material = neglected;
        }

        //If the happy threshold is met, the expression turns happy
        if (IsHappy() && meshRenderer.material != happy && !asleep)
        {
            //Debug.LogFormat("Changing face from {0} to happy.", meshRenderer.material.name);
            meshRenderer.material = happy;
        }

        //If no thresholds are met, the face is neutral
        if (!NegativeApplied() && !IsHappy() && meshRenderer.material != neutral && toggleNeedsExpressions && !asleep)
        {
            //Debug.LogFormat("Changing face from {0} to neutral.", meshRenderer.material.name);
            meshRenderer.material = neutral;
        }

        if (asleep)
        {
            meshRenderer.material = sleeping;
        }
    }
}
