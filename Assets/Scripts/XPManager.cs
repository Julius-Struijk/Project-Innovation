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
        //levelTMP.text = "Level: " + currentLevel;
    }

    private void Update()
    {
        // Update level if xp requirememnt has been surpassed.
        if(xp > startingXP + (levelDifferenceXP * currentLevel))
        {
            xp = 0f;
            currentLevel++;
            levelTMP.text = "Level: " + currentLevel;
        }

        // If the current level is higher than the lowest level threshold in the list then the player model is updated.
        if (levelThresholds != null && OnReachThreshold != null && currentLevel >= levelThresholds[0])
        {
            levelThresholds.RemoveAt(0);
            OnReachThreshold();
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

    #region

    public void Save(ref XPData data)
    {
        data.xpAmount = xp;
        data.levelAmount = currentLevel;
        //Debug.LogFormat("Saving XP data. XP: {0} Level: {1}", data.xpAmount, data.levelAmount);
    }

    public void Load(XPData data)
    {
        //Debug.LogFormat("Loading XP data. XP: {0} Level: {1}", data.xpAmount, data.levelAmount);
        xp = data.xpAmount;
        currentLevel = data.levelAmount;

        // Updating text to match loaded data
        xpTMP.text = "XP: " + (xp);
        levelTMP.text = "Level: " + currentLevel;
    }

    #endregion
}

[System.Serializable]
public struct XPData
{
    public float xpAmount;
    public int levelAmount;
}
