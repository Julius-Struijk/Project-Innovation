using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HideAndSeek : MonoBehaviour
{
    GameObject pet;
    InputHandler inputHandler;
    AudioLoudnessDetection audioDetector;
    GameManager gameManager;
    AudioSource source;
    Animator animator;

    //Assign objects to hide behind in the inspector
    [SerializeField]
    List<GameObject> hidingObjects = new List<GameObject>();
    //Sound clip for the response of the pet
    [SerializeField]
    AudioClip petResponseSound;
    //The time it takes for the pet to peek
    [SerializeField]
    float peekTime;
    //The amount the pet peeks up after the call amount has been reached
    [SerializeField]
    float peekOffset;

    //The chose object that the pet will hide behind
    GameObject chosenObject;
    //Amount of calls that have crossed the mic threshold
    int registeredCalls = 0;
    //cooldown time after a call has been registered
    [SerializeField]
    float coolDownTime;

    //bool that says if the minigame has been reset already
    bool hasBeenReset = true;

    //bool wether the pet is peeking or not
    bool petIsPeeking;
    //the position of the pet once it is in hiding
    Vector3 hidePosition;

    void Start()
    {
        animator = GetComponent<Animator>();
        source = Camera.main.GetComponent<AudioSource>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        inputHandler = gameManager.gameObject.GetComponent<InputHandler>();
        audioDetector = GameObject.FindGameObjectWithTag("AudioDetector").GetComponent<AudioLoudnessDetection>();
    }

    private void OnEnable()
    {
        if (pet == null)
        {
            pet = GameObject.FindGameObjectWithTag("Pet");
        }

        if (gameObject.activeSelf && !hasBeenReset)
        {
            ResetGame();
        }
    }

    //Choose a random object from the list for the pet to hide behind
    void ChooseHidingObject()
    {
        chosenObject = hidingObjects[Random.Range(0, hidingObjects.Count)];
    }


    float peekTimer = 0;
    //Method transitions the position between the hiding position and the given offset in a given time
    void PetPeeks(Vector3 startPos)
    {
        peekTimer += Time.deltaTime;
        if (peekTimer <= peekTime)
        {
            pet.transform.position = new Vector3(pet.transform.position.x, Mathf.Lerp(startPos.y, startPos.y + peekOffset, peekTimer / peekTime), pet.transform.position.z);
        }
    }

    //Resets all relevant things so that the game can start up again normally
    void ResetGame()
    {
        ChooseHidingObject();
        pet.transform.position = chosenObject.transform.position;
        hidePosition = pet.transform.position;
        pet.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        peekTimer = 0;
        petIsPeeking = false;
        hasBeenReset = true;
    }

    //Plays shaking animation of the bush that the pet is hiding behind
    void ShakeBush()
    {
        switch (hidingObjects.IndexOf(chosenObject)) 
        {
            case 0:
                animator.Play("shakeZero");
                break;
            case 1:
                animator.Play("shakeOne");
                break;
            case 2:
                animator.Play("shakeTwo");
                break;
            default:
                break;
        }
    }

    //Checks if the correct hiding spot is tapped
    void CheckTaps()
    {
        if (inputHandler.interact.WasPerformedThisFrame())
        {
            RaycastHit2D hit = Physics2D.Raycast(inputHandler.GetScreenToWorldPos(Pointer.current.position.ReadValue()), Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject == chosenObject)
            {
                gameManager.CompleteHideAndSeek();
            }
        }
    }

    //Reset the game once it gets active again
    private void OnDisable()
    {
        hasBeenReset = false;
        registeredCalls = 0;
    }

    //Last time a call was registered
    float lastCallTime = 0;

    void Update()
    {
        //registers calls when the mic threshold is met and there is no cooldown 
        if (audioDetector.GetLoudnessFromMicrophone() >= gameManager.micResponseThreshold && Time.time >= lastCallTime + coolDownTime && registeredCalls < gameManager.amountOfCallsNeeded)
        {   
            registeredCalls++;
            lastCallTime = Time.time;
            ShakeBush();
            Debug.Log("Call Registered: " + registeredCalls);
        }

        //Make the pet peek once the amount of calls needed is met
        if (registeredCalls == gameManager.amountOfCallsNeeded  && !petIsPeeking)
        {
            Debug.Log("Triggering pet response");
            source.clip = petResponseSound;
            source.Play();
            petIsPeeking = true;
        }

        //Check for taps once the pet is peeking
        if (petIsPeeking)
        {
            PetPeeks(hidePosition);
            CheckTaps();
        }
    }
}
