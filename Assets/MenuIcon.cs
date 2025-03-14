using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuIcon : MonoBehaviour
{
    GameManager gameManager;
  
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }
    void Update()
    {
        if (gameManager.MinigameOngoing() && gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }
}
