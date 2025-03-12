using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class XPManager : MonoBehaviour
{
    // Showcase XP amount for debugging
    [SerializeField] TextMeshProUGUI xpTMP, levelTMP;

    // These are serialized for debugging purposes.
    [SerializeField] int currentLevel = 1;
    [SerializeField] float xpMultiplier = 1.0f;
    float needsFilled = 4;

    [SerializeField] List<int> levelThresholds;
    public static event Action OnReachThreshold;
    [SerializeField] float startingXP = 1000f;
    [SerializeField] float levelDifferenceXP = 500f;
    [SerializeField]
    Slider xpSlider;
    [SerializeField]
    float xp;

    // Start is called before the first frame update
    void Start()
    {
        levelTMP.text = currentLevel.ToString();

        if (xpSlider != null)
        {
            xpSlider.maxValue = levelDifferenceXP;
        }
    }


    float oldXPValue = 0;
    private void Update()
    {
        // Update level if xp requirememnt has been surpassed.
        if (xp >= levelDifferenceXP)
        {
            Debug.Log("Level up");
            xp = xp - levelDifferenceXP;
            currentLevel++;
            levelTMP.text = currentLevel.ToString();
        }

        // If the current level is higher than the lowest level threshold in the list then the player model is updated.
        if (levelThresholds != null && OnReachThreshold != null && currentLevel >= levelThresholds[0])
        {
            levelThresholds.RemoveAt(0);
            OnReachThreshold();
        }

        if (oldXPValue != xp)
        {
            UpdateXPBar();
            oldXPValue = xp;
        }

       
    }

    public void AddXP(float givenXP)
    {
        xp += (givenXP * xpMultiplier);
        if (xpTMP != null) { xpTMP.text = "XP: " + (xp); }
    }

    public void NeedsFilled(int needChange)
    {
        needsFilled += needChange;

        // Update XP multiplier based on how many needs have been filled.
        switch (needsFilled)
        {
            case 0:
                ChangeMultiplier(1.0f);
                break;
            case 1:
                ChangeMultiplier(1.15f);
                break;
            case 2:
                ChangeMultiplier(1.30f);
                break;
            case 3:
                ChangeMultiplier(1.45f);
                break;
            case 4:
                ChangeMultiplier(2.0f);
                break;
            default:
                break;
        }
    }

    void UpdateXPBar()
    {
        xpSlider.value = xp;
        Debug.Log("Updated to: " + xpSlider.value);
    }

    private void ChangeMultiplier(float newMultiplier)
    {
        Debug.LogFormat("Updated multiplier to: {0}", newMultiplier);
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
