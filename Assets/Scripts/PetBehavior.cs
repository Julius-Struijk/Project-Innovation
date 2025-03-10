using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PetBehavior : MonoBehaviour
{
    //Reference to game manager
    GameManager gameManager;
    //Reference to input handler
    InputHandler inputHandler;
    //Default position of the pet
    Vector3 defaultPos;
    //Reference to ui manager
    UIManager uiManager;
    //Default scaling of the pet
    Vector3 defaultScale;
    //Position of the pet in the bedroom
    [SerializeField]
    Vector3 bedroomPos;
    //The current fact texture
    [SerializeField]
    Material currentExpression;
    //Scaler multiplier for bedroom
    [SerializeField]
    float bedroomScalar;

    //For tweaking the movement speed in inspector
    [SerializeField]
    float minMovementSpeed;
    [SerializeField]
    float maxMovementSpeed;
    //The movement speed that gets exhibited
    float movementSpeed;

    //Bools to track which way the pet is moving
    bool movingRight = true;
    bool movingLeft = false;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        inputHandler = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputHandler>();
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();

        defaultPos = transform.position;
        defaultScale = transform.localScale;

        //Make sure the pet has a random movement speed to start with
        RandomizeMovementSpeed();
    }

    //Behavior the pet should have during the cleaning minigame
    void CleaningGameBehavior()
    {
        //Make the pet move left or right accordingly
        if (movingRight)
        {
            transform.position += new Vector3(movementSpeed * Time.deltaTime, 0, 0);
        }

        if (movingLeft)
        {
            transform.position -= new Vector3(movementSpeed * Time.deltaTime, 0, 0);
        }

        //If the pet hits the screen's edge, invert its direction and randomize the movement speed
        if (transform.position.x < inputHandler.ScreenMinimumX() || transform.position.x > inputHandler.ScreenMaximumX())
        {
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, inputHandler.ScreenMinimumX(), inputHandler.ScreenMaximumX()), transform.position.y, transform.position.z);
            ToggleMoveDirection();
            RandomizeMovementSpeed();
        } 
    }


    //Method to change the direction the pet is moving in
    void ToggleMoveDirection()
    {
        if (movingLeft)
        {
            movingRight = true;
            movingLeft = false;
            return;
        }
        else
        {
            movingLeft = true;
            movingRight = false;
        }
    }

    //Method that randomizes the movement speed
    void RandomizeMovementSpeed()
    {
        movementSpeed = Random.Range(minMovementSpeed, maxMovementSpeed);
    }

    void Update()
    {
        //If the game manager says the cleaning game is ongoing, exhibit its behavior
        if (gameManager.cleaningGameOngoing)
        {
            CleaningGameBehavior();
        }
        
        //If there are no minigames going on and we're not in the bedroom, reset to default pos
        if (transform.position != defaultPos && !gameManager.MinigameOngoing() && gameManager.currentRoom != "Bedroom")
        {
            //Reset pet position and scale
            transform.position = defaultPos;
            transform.localScale = defaultScale;
        }

        if (gameManager.currentRoom == "Bedroom" && transform.position != bedroomPos)
        {
            transform.position = bedroomPos;
            transform.localScale = defaultScale * bedroomScalar;
        }
    }
}
