using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Reference to the pet/Zibbs
    [SerializeField]
    GameObject pet;
    //Reference to input handler
    InputHandler inputHandler;
    //String to keep track of which room the user is in
    public string currentRoom;
    //The empty gameobject that holds all cleaning minigame objects, assign in inspector
    [SerializeField]
    GameObject cleaningGameObject;
    //Bool to track if the cleaning game is currently ongoing
    public bool cleaningGameOngoing = false;
    //The amount of droplets that should hit the pet to complete the cleaning minigame
    public int cleaningGameHitThreshold;
    //The amount of droplets that have currently hit the pet
    [HideInInspector]
    public int cleaningGameHitAmount = 0;


    void Start()
    {
        inputHandler = GetComponent<InputHandler>();
        cleaningGameObject.SetActive(false);
        currentRoom = "Bathroom";
    }

    public void StartCleaningMinigame()
    {
        cleaningGameObject.SetActive(true);
        cleaningGameOngoing = true;
    }

    public void CleaningGameComplete()
    {
        cleaningGameObject.SetActive(false);
        cleaningGameHitAmount = 0;
        cleaningGameOngoing = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(cleaningGameHitAmount >= cleaningGameHitThreshold)
        {
            CleaningGameComplete();
        }
    } 
}
