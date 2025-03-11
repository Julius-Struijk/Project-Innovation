using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.Events;
using Unity.VisualScripting;
using UnityEditor.Search;

public class UIManager : MonoBehaviour
{
    //Reference to the input script
    [SerializeField]
    InputHandler inputHandler;
    //Reference to the game manager script
    [SerializeField]
    GameManager gameManager;
    //The object that holds the needs bars UI
    [SerializeField]
    GameObject needsBarsObject;
    //The object that holds all bathroom UI, assign in inspector
    [SerializeField]
    GameObject bathroomUI;
    //The object that holds the kitchen UI, assign in inspector
    [SerializeField]
    GameObject bedroomUI;
    //Garden UI object
    [SerializeField]
    GameObject gardenUI;

    //List that holds all the UI objects, used to switch between them
    List<GameObject> roomUIObjects = new List<GameObject>();


    //The values for the different needs, set their starting values in the inspector
    public float hungerValue;
    public float happinessValue;
    public float energyValue;
    public float healthValue;

    [HideInInspector]
    public float maxHunger, maxHappiness, maxEnergy, maxHealth;

    //The time it takes for a need bar to drop by 1
    [SerializeField]
    float needsDepletionInterval;

    private void Awake()
    {
        maxHunger = hungerValue;
        maxHealth = healthValue;
        maxEnergy = energyValue;
        maxHappiness = happinessValue;
    }

    void Start()
    {
        roomUIObjects.Add(bathroomUI);
        roomUIObjects.Add(bedroomUI);
        roomUIObjects.Add(gardenUI);
        RoomUISwitch("Bathroom");
    }

    //Methodd for the UI to switch to a certain room
    public void RoomUISwitch(string roomName)
    {
        //Determine the UI to switch to base on the room string
        GameObject uiToSwitch = null;

        switch (roomName)
        {
            case "Bathroom":
                uiToSwitch = bathroomUI;
                break;
            case "Garden":
                uiToSwitch = gardenUI;
                break;
            case "Bedroom":
                uiToSwitch = bedroomUI;
                break;
            default:
                Debug.Log("string not valid");
                break;
        }

        //Set the relevant UI to active and disable the others
        foreach (GameObject roomUI in roomUIObjects)
        {
            if (roomUI == uiToSwitch)
            {
                roomUI.SetActive(true);
            }
            else
            {
                roomUI.SetActive(false);
            }
        }
    }

    //Addition methods for the different needs values
    public void AddHunger(float value)
    {
        hungerValue += value;
        hungerValue = Mathf.Clamp(hungerValue, 0, 100);
    }

    public void AddHealth(float value)
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


    //Progress bar for the cleaning minigame
    Slider cleaningProgressSlider;
    GameObject cleaningMinigameBackground;
    void HandleBathroomUI()
    {
        //Find the slider
        if (cleaningProgressSlider == null)
        {
            cleaningProgressSlider = bathroomUI.transform.Find("Cleaning Progress Bar").gameObject.GetComponent<Slider>();
        }

        if (cleaningMinigameBackground == null)
        {
            cleaningMinigameBackground = bathroomUI.transform.Find("Minigame BG").gameObject;
        }

        if (!gameManager.cleaningGameOngoing && !bathroomUI.transform.Find("Cleaning button").gameObject.activeSelf)
        {
            bathroomUI.transform.Find("Cleaning button").gameObject.SetActive(true);
        }

        if (gameManager.cleaningGameOngoing)
        {
            cleaningProgressSlider.gameObject.SetActive(true);
            cleaningMinigameBackground.SetActive(true);
        }
        else
        {
            cleaningMinigameBackground.SetActive(false);
            cleaningProgressSlider.gameObject.SetActive(false);
        }

        cleaningProgressSlider.value = (float)gameManager.cleaningGameHitAmount / gameManager.cleaningGameHitThreshold;
    }


    GameObject hideAndSeekButton;
    GameObject hideAndSeekBackground;
    void HandleGardenUI()
    {
        if (hideAndSeekButton == null)
        {
            hideAndSeekButton = gardenUI.transform.Find("Hide And Seek Button").gameObject;
        }

        if (hideAndSeekBackground == null)
        {
            hideAndSeekBackground = gardenUI.transform.Find("Minigame BG").gameObject;
        }

        if (gameManager.hideAndSeekOngoing)
        {
            hideAndSeekButton.SetActive(false);
            hideAndSeekBackground.SetActive(true);
        }
        else
        {
            hideAndSeekButton.SetActive(true);
            hideAndSeekBackground.SetActive(false);
        }
    }


    float lastDepletionTime = 0;

    void HandleNeeds()
    {
        if (lastDepletionTime == 0 && Time.time > needsDepletionInterval)
        {
            AddEnergy(-1);
            AddHunger(-1);
            AddHappiness(-1);
            AddHealth(-1);

            lastDepletionTime = Time.time;
        }
        else if (Time.time - lastDepletionTime > needsDepletionInterval)
        {
            AddEnergy(-1);
            AddHunger(-1);
            AddHappiness(-1);
            AddHealth(-1);

            lastDepletionTime = Time.time;
        }
}

    private void Update()
    {
        if (gameManager.currentRoom == "Bathroom")
        {
            HandleBathroomUI();
        }

        if (gameManager.currentRoom == "Garden")
        {
            HandleGardenUI();
        }

        if (gameManager.MinigameOngoing() && needsBarsObject.activeSelf)
        {
            needsBarsObject.SetActive(false);
        }

        if (!gameManager.MinigameOngoing() && !needsBarsObject.activeSelf)
        {
            needsBarsObject.SetActive(true);
        }

        HandleNeeds();
    }
}
