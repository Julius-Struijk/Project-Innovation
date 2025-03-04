using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Reference to the pet/Zibbs
    [SerializeField]
    GameObject pet;
    //Reference to the audio loudness detector, assign in inspector
    [SerializeField]
    AudioLoudnessDetection audioDetector;
    //Reference to input handler
    InputHandler inputHandler;
    //Reference to UI manager
    UIManager uiManager;
    //String to keep track of which room the user is in
    public string currentRoom;

    //The empty gameobject that holds all cleaning minigame objects, assign in inspector
    [SerializeField]
    GameObject cleaningGameObject;
    //Bool to track if the cleaning game is currently ongoing
    [HideInInspector]
    public bool cleaningGameOngoing = false;
    //The amount of droplets that should hit the pet to complete the cleaning minigame
    public int cleaningGameHitThreshold;
    //The amount of droplets that have currently hit the pet
    [HideInInspector]
    public int cleaningGameHitAmount = 0;

    //The object that holds the hide and seek gameobjects, assign in inspector
    [SerializeField]
    GameObject hideAndSeekObject;
    //Hide and seek bool that tracks if the minigame is ongoing
    [HideInInspector]
    public bool hideAndSeekOngoing = false;
    //The mic threshold the player needs to reach to trigger a response
    public float micResponseThreshold;
    //Amount of times th player needs to call out to Zibbs
    public int amountOfCallsNeeded;

    


    void Start()
    {
        //Finding handlers
        inputHandler = GetComponent<InputHandler>();
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();

        //Deactivating the minigame objects
        cleaningGameObject.SetActive(false);
        hideAndSeekObject.SetActive(false);

        //current way to start at a certain room, subject to change
        ChangeCurrentRoom("Bathroom"); 
    }

    //Method to start the cleaning minigame
    public void StartCleaningMinigame()
    {
        cleaningGameObject.SetActive(true);
        cleaningGameOngoing = true;
    }

    //Method to call once the cleaning game is completed
    public void CleaningGameComplete()
    {
        cleaningGameObject.SetActive(false);
        cleaningGameHitAmount = 0;
        cleaningGameOngoing = false;
    }

    //Starts the hide and seek minigame
    public void StartHideAndSeek()
    {
        hideAndSeekObject.SetActive(true);
        hideAndSeekOngoing = true;
        audioDetector.StartMicRecording();
    }

    //Completes the hide and seek minigame
    public void CompleteHideAndSeek()
    {
        hideAndSeekObject.SetActive(false);
        hideAndSeekOngoing = false;
        audioDetector.EndMicRecording();
    }

    //Change the current room and its UI
    public void ChangeCurrentRoom(string roomName)
    {
        currentRoom = roomName;
        uiManager.RoomUISwitch(roomName);
    }

    public bool MinigameOngoing()
    {
        if (cleaningGameOngoing || hideAndSeekOngoing)
        {
            return true;
        }
        else return false;
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
