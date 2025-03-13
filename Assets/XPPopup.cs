using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class XPPopup : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI levelProgressText;
    XPManager xpManager;

    private void Start()
    {
        xpManager = GameObject.FindGameObjectWithTag("XPManager").GetComponent<XPManager>();
        if (levelProgressText == null) { Debug.Log("Please assign level progress text"); }
        if (xpManager == null) { Debug.Log("Could not find xp manager"); }

        levelProgressText.text = "XP until level " + (xpManager.currentLevel + 1) + ": " + xpManager.xp + "/" + xpManager.levelDifferenceXP;
    }

    float lastXPValue = 0;
    private void Update()
    {
        if (xpManager.xp != lastXPValue)
        {
            levelProgressText.text = "XP until level " + (xpManager.currentLevel + 1) + ": " + xpManager.xp + "/" + xpManager.levelDifferenceXP;
            lastXPValue = xpManager.xp;
        }
    }
}
