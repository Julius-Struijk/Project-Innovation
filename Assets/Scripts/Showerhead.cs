using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Showerhead : MonoBehaviour
{
    //Reference to the water droplet prefab, assign in inspector
    [SerializeField]
    GameObject dropletPrefab;
    //Input handler reference
    InputHandler inputHandler;
    //The object's position on the screen
    Vector2 screenPos;
    //Time interval of when droplets should spawn
    [SerializeField]
    float dropletIntervalSeconds;

    void Start()
    {
        inputHandler = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputHandler>();

        if (inputHandler == null)
        {
            Debug.LogError("GameManager not found");
        }
    }

    float lastSpawnTime = 0;
    float elapsedTime = 0;
    void SpawnDroplets()
    {
        elapsedTime += Time.fixedDeltaTime;

        if(elapsedTime - lastSpawnTime >= dropletIntervalSeconds)
        {
            Instantiate(dropletPrefab, transform.position, Quaternion.identity, transform.parent);
            lastSpawnTime = elapsedTime;
        }
    }

    //Make sure the showerhead cannot move out of screen bounds
    void ConstrainPositionToScreen()
    {
        if (screenPos.x < 0 || screenPos.x > inputHandler.GetCameraWidth())
        {
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, inputHandler.ScreenMinimumX(), inputHandler.ScreenMaximumX()), transform.position.y, transform.position.z);
        }
    }


    void Update()
    {
        //If the showerhead was moved this frame, update the screen position
        if (inputHandler.movement.WasPerformedThisFrame())
        {          
            transform.position += new Vector3(Accelerometer.current.acceleration.ReadValue().x, 0, 0);
            screenPos = inputHandler.GetObjectScreenPos(gameObject);          
        }


        ConstrainPositionToScreen();
        SpawnDroplets();
    }
}
