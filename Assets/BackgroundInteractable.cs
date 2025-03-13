using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundInteractable : MonoBehaviour
{
    Button button;
    GameManager gameManager;

    private void Start()
    {
        button = GetComponent<Button>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        if (button == null) { Debug.Log("gameobject does not have button component"); }
        if (gameManager == null) { Debug.Log("could not find game manager"); }
    }

    private void Update()
    {
        if (gameManager.backgroundInteractable && !button.enabled)
        {
            button.enabled = true;
        }

        if (!gameManager.backgroundInteractable && button.enabled)
        {
            button.enabled = false;
        }
    }
}
