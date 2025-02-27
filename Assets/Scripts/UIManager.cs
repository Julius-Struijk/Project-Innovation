using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    //Reference to the input script
    [SerializeField]
    InputHandler inputHandler;

    //The values for the different needs, set their starting values in the inspector
    public float hungerValue;
    public float happinessValue;
    public float energyValue;
    public float healthValue;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    //Addition methods for the different needs values
    public void AddHunger(float value)
    {
        hungerValue += value;
        hungerValue = Mathf.Clamp(hungerValue, 0, 100);
    }

    public void HealthHunger(float value)
    {
        healthValue += value;
        healthValue = Mathf.Clamp(healthValue, 0, 100);
    }

    public void AddHappiness(float value)
    {
        happinessValue += value;
        happinessValue = Mathf.Clamp(happinessValue, 0, 100);
    }

    public void AddEnergy(float value)
    {
        energyValue += value;
        energyValue = Mathf.Clamp(energyValue, 0, 100);
    }
}
