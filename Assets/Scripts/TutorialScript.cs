using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour
{
    //References to the needed scripts
    [SerializeField]
    ChangeExpressions expressions;
    [SerializeField]
    SwitchRooms roomSwitching;
    [SerializeField]
    GameManager gameManager;
    //References to popups that get activated using the script
    [SerializeField]
    GameObject feedingPopup, playingPopup, callPopup, dirtyPopup, cleanPopup, tiredPopup, sleepPopup;

    //Room to lock into
    string targetRoom;
    //Phase of the tutorial
    string currentPhase = "waking";
    //If the tutorial is currently locked in a room
    bool locked = false;

    void Start()
    {
        //null checks
        if (expressions == null)
        {
            Debug.LogError("Please assign expressions script");
        }
        else
        {
            expressions.ChangeExpression("Sleeping");
        }

        if (roomSwitching == null)
        {
            Debug.LogError("Please assign roomswitch script");
        }

        if (gameManager == null)
        {
            Debug.LogError("Please assign gameManager script");
        }

        if (feedingPopup == null)
        {
            Debug.LogError("Please assing feeding popup");
        }

        if (playingPopup == null)
        {
            Debug.LogError("Please assing playing popup");
        }

        if (callPopup == null) { Debug.LogError("Please assign call popup"); }
        if (dirtyPopup == null) { Debug.LogError("Please assign dirty popup"); }
        if (cleanPopup == null) { Debug.LogError("Please assign clean popup"); }
        if (tiredPopup == null) { Debug.LogError("Please assign clean popup"); }
        if (sleepPopup == null) { Debug.LogError("Please assign sleep popup"); }

        expressions.currentMaterial = "sleeping";
        expressions.ChangeExpression("sleeping");    
    }

    public void SetPhase(string phaseName)
    {
        currentPhase = phaseName;
    }

    public void SetRoomTarget(string roomName)
    {
        targetRoom = roomName;
    }

    void Update()
    {
        //Logic to make the screens progress as they should

        if (gameManager.currentRoom == targetRoom && !locked)
        {
            if (targetRoom == "Kitchen" && currentPhase == "feeding")
            {
                feedingPopup.SetActive(true);
                locked = true;
            }

            if (targetRoom == "Garden" && currentPhase == "playing")
            {
                playingPopup.SetActive(true);
                locked = true;
            }

            if (targetRoom == "Bathroom" && currentPhase == "cleaning")
            {
                cleanPopup.SetActive(true);
            }

            if(targetRoom == "Bedroom" && currentPhase == "tired")
            {
                sleepPopup.SetActive(true);
            }

            if (roomSwitching.swipeEnabled)
            {
                roomSwitching.DisableSwiping();
            }
        }

        if (roomSwitching.swipeEnabled && locked)
        {
            locked = false;
        }

        if (gameManager.hideAndSeekOngoing && currentPhase == "playing")
        {
            callPopup.SetActive(true);
        }

        if (currentPhase == "hide and seek" && !gameManager.hideAndSeekOngoing)
        {
            dirtyPopup.SetActive(true);
        }

        if (currentPhase == "showering" & !gameManager.cleaningGameOngoing)
        {
            tiredPopup.SetActive(true);
        }

    }
}
