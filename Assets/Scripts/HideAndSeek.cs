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
    //The time without response that it takes for the mic threshold to be lowered
    [SerializeField]
    float micAdjustTime;

    //The chose object that the pet will hide behind
    GameObject chosenObject;
    //Amount of calls that have crossed the mic threshold
    int registeredCalls = 0;
    //cooldown time after a call has been registered
    [SerializeField]
    float coolDownTime;
    //reference to the standard mic threshold from the game manager
    float _micThreshold;
    //The time at which the minigame started
    float gameStartTime;
    //The last recorded time the mic has been adjusted 
    float lastMicAdjustment = 0;

    //bool that says if the minigame has been reset already
    bool hasBeenReset = true;

    //bool wether the pet is peeking or not
    bool petIsPeeking;
    //the position of the pet once it is in hiding
    Vector3 hidePosition;

    //Audio
    public AudioClip[] audioClips;
    [Range(0f, 1f)]
    public float clipVolume = 1.0f;

    void Start()
    {
        animator = GetComponent<Animator>();
        source = Camera.main.GetComponent<AudioSource>();
        inputHandler = gameManager.gameObject.GetComponent<InputHandler>();
        audioDetector = GameObject.FindGameObjectWithTag("AudioDetector").GetComponent<AudioLoudnessDetection>();


    }

    private void OnEnable()
    {
        if (pet == null)
        {
            pet = GameObject.FindGameObjectWithTag("Pet");
        }

        if (gameManager == null)
        {
            gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            _micThreshold = gameManager.micResponseThreshold;
            lastMicAdjustment = Time.time;
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
        pet.transform.position = chosenObject.transform.position + new Vector3(0, -0.5f, 2);
        if (gameManager.micResponseThreshold != _micThreshold)
        {
            gameManager.micResponseThreshold = _micThreshold;
        }
        lastMicAdjustment = 0;
        lastCallTime = 0;
        gameStartTime = Time.time;
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
                AudioManager.Instance.ButtonInteractSound();
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

    //Checks if if the last registered call time exceeds the adjusting cooldown
    bool LastCallTimeExceeded()
    {
        //if the last call time is 0, use the starttime of the game instead
        if(lastCallTime == 0 && Time.time - gameStartTime > micAdjustTime)
        {
            return true;
        }

        if(lastCallTime != 0 && Time.time - lastCallTime > micAdjustTime)
        {
            return true;
        }

        return false;
    }

    //Checks if if the last mic adjustment time exceeds the adjusting cooldown
    bool LastAdjustmentTimeExceeded()
    {
        if (Time.time - lastMicAdjustment > micAdjustTime)
        {
            return true;
        }

        return false;
    }


    void Update()
    {
        //lower the mic threshold if it takes too long to register a call
        if (registeredCalls != gameManager.amountOfCallsNeeded && gameManager.micResponseThreshold > 2)
        { 
            if (LastCallTimeExceeded() && LastAdjustmentTimeExceeded())
            {
                gameManager.micResponseThreshold -= 2;
                lastMicAdjustment = Time.time;
            }
        }

        //registers calls when the mic threshold is met and there is no cooldown 
        if (audioDetector.GetLoudnessFromMicrophone() >= gameManager.micResponseThreshold && Time.time >= lastCallTime + coolDownTime && registeredCalls < gameManager.amountOfCallsNeeded)
        {
            registeredCalls++;
            lastCallTime = Time.time;
            ShakeBush();

            //play audio clip
            AudioManager.Instance.PlaySound(audioClips[Random.Range(0, audioClips.Length)], clipVolume);

            Debug.Log("Call Registered: " + registeredCalls);
        }

        //Make the pet peek once the amount of calls needed is met
        if (registeredCalls == gameManager.amountOfCallsNeeded && !petIsPeeking)
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
