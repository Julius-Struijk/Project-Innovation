using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class NeedsBar : MonoBehaviour
{
    //Enum of trackable needs, selected the desired one in the inspector
    enum Needs { Hunger, Happiness, Energy, Health }
    [SerializeField]
    Needs selectedNeed;

    //Assign the slider component of the bar in the inspector
    [SerializeField]
    Slider slider;
    //Assign the bar that will be depleting in the inspector
    [SerializeField]
    Image bar;
    //Gradient for changing the color of the bar, can be edited in the inspector
    [SerializeField]
    Gradient gradient;

    //The UIManager value that the bar will be tracking
    float TrackedValue()
    {
        switch (selectedNeed)
        {
            case Needs.Hunger:
                return uiManager.hungerValue;
            case Needs.Happiness:
                return uiManager.happinessValue;
            case Needs.Energy:
                return uiManager.energyValue;
            case Needs.Health:
                return uiManager.hungerValue;
            default:
                return 0;
        }
    }

    //Reference to the uiManager
    UIManager uiManager;

    void Start()
    {
        //Getting the UIManager script
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();

        //Null checks
        if (uiManager == null)
        {
            Debug.LogError("Could not find any object with tag 'UIManager'");
        }
        if (slider == null)
        {
            Debug.LogError("Please assign a slider in the inspector");
        }


    }

    //Method that updates the bar according to the tracked needs value
    void UpdateBar()
    {
        slider.value = TrackedValue();
        bar.color = gradient.Evaluate(0.01f * TrackedValue());
    }

    void Update()
    {
        //Update the bar if its tracked value changes
        if (ValueChanged())
        {
            UpdateBar();
        }        
    }

    //Value for tracking the last recorded update of the tracked value
    float oldValue = -1;
    //Boolean that returns true if the tracked value changes
    bool ValueChanged()
    {
        if (oldValue != TrackedValue())
        {
            oldValue = TrackedValue();
            return true;
        }
        else return false;
    }
}
