using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAging : MonoBehaviour
{
    [SerializeField] List<GameObject> ageModels;
    int ageCounter = 0;

    void Start()
    {
        XPManager.OnReachThreshold += AgeCharacter;
    }

    void AgeCharacter()
    {
        // Switching character model to the older one.
        if (ageModels.Count > ageCounter)
        {
            ageModels[ageCounter].SetActive(false);
            ageCounter++;
            ageModels[ageCounter].SetActive(true);
        }
    }

    private void OnDestroy()
    {
        XPManager.OnReachThreshold -= AgeCharacter;
    }
}
