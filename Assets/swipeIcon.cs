using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class swipeIcon : MonoBehaviour
{
    SwitchRooms roomSwitching;
    Image image;
    GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        roomSwitching = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SwitchRooms>();
        image = GetComponent<Image>();
        

        if (roomSwitching == null || gameManager ==  null) { Debug.Log("room switch script/game manager not found"); }
        if (image == null) { Debug.Log("image component not found"); }
    }


    void Update()
    {
        if (roomSwitching.swipeEnabled)
        {
            if (!image.enabled && !gameManager.MinigameOngoing())
            {
                image.enabled = true;
            }
        }

        if (!roomSwitching.swipeEnabled || gameManager.MinigameOngoing())
        {
            if (image.enabled)
            {
                image.enabled = false;
            }
        }
    }
}
