using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    [SerializeField] GameObject changingObject;
    [HideInInspector]
    public bool changingOngoing = false;
    //Enables/disables the interactability of the outlined background buttons
    public bool backgroundInteractable = true;

    XPManager xpManager;
    [SerializeField] float cleaningXP = 200f;
    [SerializeField] float hideXP = 300f;

    private void Awake()
    {
        //Application.runInBackground = true;
        //DontDestroyOnLoad(gameObject);
    }

    public void MakeBackgroundUninteractable()
    {
        backgroundInteractable = false;
    }

    public void MakeBackgroundInteractable()
    {
        backgroundInteractable = true;
    }

    void Start()
    {
        //Finding handlers
        inputHandler = GetComponent<InputHandler>();
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        xpManager = GameObject.FindGameObjectWithTag("XPManager").GetComponent<XPManager>();

        //Deactivating the minigame objects
        cleaningGameObject.SetActive(false);
        hideAndSeekObject.SetActive(false);
        if (changingObject != null) { changingObject.SetActive(false); }


        ChangeCurrentRoom("Bedroom");
    }

    //Method to start the cleaning minigame
    public void StartCleaningMinigame()
    {
        if (backgroundInteractable)
        {
            cleaningGameObject.SetActive(true);
            cleaningGameOngoing = true;
        }
    }

    //Method to call once the cleaning game is completed
    public void CleaningGameComplete()
    {
        cleaningGameObject.SetActive(false);
        uiManager.AddHealth(30);
        cleaningGameHitAmount = 0;
        cleaningGameOngoing = false;
        if (xpManager != null) { xpManager.AddXP(cleaningXP); }
    }

    //Starts the hide and seek minigame
    public void StartHideAndSeek()
    {
        if (backgroundInteractable)
        {
            hideAndSeekObject.SetActive(true);
            hideAndSeekOngoing = true;
            audioDetector.StartMicRecording();
        }
    }

    //Completes the hide and seek minigame
    public void CompleteHideAndSeek()
    {
        uiManager.AddHappiness(30);
        hideAndSeekObject.SetActive(false);
        hideAndSeekOngoing = false;
        audioDetector.EndMicRecording();
        if (xpManager != null) { xpManager.AddXP(hideXP); }
    }

    public void CancelHideAndSeek()
    {
        Debug.Log("cancelling");
        hideAndSeekObject.SetActive(false);
        hideAndSeekOngoing = false;
        audioDetector.EndMicRecording();
    }

    public void CancelCleaningGame()
    {
        cleaningGameHitAmount = 0;
        cleaningGameOngoing = false;
        cleaningGameObject.SetActive(false);
    }

    public void StartChanging()
    {
        if (backgroundInteractable)
        {
            changingObject.SetActive(true);
            changingOngoing = true;
        }
    }

    public void CompleteChanging()
    {
        changingObject.SetActive(false);
        changingOngoing = false;
    }

    //Change the current room and its UI
    public void ChangeCurrentRoom(string roomName)
    {
        currentRoom = roomName;
        if (uiManager != null)
        {
            uiManager.RoomUISwitch(roomName);
        }
    }

    public bool MinigameOngoing()
    {
        if (cleaningGameOngoing || hideAndSeekOngoing || changingOngoing)
        {
            return true;
        }
        else return false;
    }

    // Update is called once per frame
    void Update()
    {
        if (cleaningGameHitAmount >= cleaningGameHitThreshold)
        {
            CleaningGameComplete();
        }
    }

    public void QuitGame()
    {
        Application.Quit();
        //fallback
        System.Diagnostics.Process.GetCurrentProcess().Kill();
    }
} 
