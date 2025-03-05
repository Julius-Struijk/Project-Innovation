using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideAndSeek : MonoBehaviour
{
    GameObject pet;
    AudioLoudnessDetection audioDetector;
    GameManager gameManager;
    AudioSource source;

    //Assign objects to hide behind in the inspector
    [SerializeField]
    List<GameObject> hidingObjects = new List<GameObject>();
    //Sound clip for the response of the pet
    [SerializeField]
    AudioClip petResponseSound;
    //The time it takes for the pet to peek
    [SerializeField]
    float peekTime;

    //The chose object that the pet will hide behind
    GameObject chosenObject;
    //Amount of calls that have crossed the mic threshold
    int registeredCalls = 0;
    //cooldown time after a call has been registered
    [SerializeField]
    float coolDownTime;

    //bool that says if the minigame has been reset already
    bool hasBeenReset = true;

    bool petIsPeeking;

    void Start()
    {
        pet = GameObject.FindGameObjectWithTag("Pet");
        source = Camera.main.GetComponent<AudioSource>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        audioDetector = GameObject.FindGameObjectWithTag("AudioDetector").GetComponent<AudioLoudnessDetection>();
    }


    //Choose a random object from the list for the pet to hide behind
    void ChooseHidingObject()
    {
        chosenObject = hidingObjects[Random.Range(0, hidingObjects.Count - 1)];
    }

    float peekTimer = 0;
    void PetPeeks(Vector3 startPos)
    {
        peekTimer += Time.deltaTime;
        if (peekTimer <= peekTime)
        {
            pet.transform.position = new Vector3(pet.transform.position.x, Mathf.Lerp(startPos.y, startPos.y + 2f, peekTimer / peekTime), pet.transform.position.z);
        }
    }

    void ResetGame()
    {
        ChooseHidingObject();
        pet.transform.position = chosenObject.transform.position;
        pet.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        hasBeenReset = true;
    }

    private void OnDisable()
    {
        hasBeenReset = false;
        registeredCalls = 0;

    }

    //Last time a call was registered
    float lastCallTime = 0;
    void Update()
    {
        if (audioDetector.GetLoudnessFromMicrophone() >= gameManager.micResponseThreshold && Time.time >= lastCallTime + coolDownTime && registeredCalls < gameManager.amountOfCallsNeeded)
        {   
            registeredCalls++;
            lastCallTime = Time.time;
            Debug.Log("Call Registered: " + registeredCalls);
        }

        if (registeredCalls == gameManager.amountOfCallsNeeded  && !petIsPeeking)
        {
            Debug.Log("Triggering pet response");
            source.clip = petResponseSound;
            source.Play();
            PetPeeks(pet.transform.position);
            petIsPeeking = true;
        }

        if (gameObject.activeSelf && !hasBeenReset)
        {
            ResetGame();
        }
    }
}
