using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class XPManager : MonoBehaviour
{
    // Showcase XP amount for debugging
    [SerializeField] TextMeshProUGUI xpTMP, levelTMP;

    // These are serialized for debugging purposes.
    [SerializeField] int currentLevel = 1;
    [SerializeField] float xpMultiplier = 1.0f;

    [SerializeField] List<int> levelThresholds;
    public static event Action OnReachThreshold;
    [SerializeField] float startingXP = 1000f;
    [SerializeField] float levelDifferenceXP = 500f;
    float xp;

    // Start is called before the first frame update
    void Start()
    {
        levelTMP.text = "Level: " + currentLevel;
    }

    private void Update()
    {
        // Update level if xp requirememnt has been surpassed.
        if(xp > startingXP + (levelDifferenceXP * currentLevel))
        {
            xp = 0f;
            currentLevel++;
            levelTMP.text = "Level: " + currentLevel;

            // Check if level thresholds required for aging up have been reached.
            if (levelThresholds != null && OnReachThreshold != null && currentLevel == levelThresholds[0]) 
            {
                levelThresholds.RemoveAt(0);
                OnReachThreshold(); 
            }
        }
    }

    public void AddXP(float givenXP)
    {
        xp += (givenXP * xpMultiplier);
        xpTMP.text = "XP: " + (xp);
    }

    public void ChangeMultiplier(float newMultiplier)
    {
        xpMultiplier = newMultiplier;
    }
}
