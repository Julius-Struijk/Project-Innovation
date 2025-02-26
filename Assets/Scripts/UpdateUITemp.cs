using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateUITemp : MonoBehaviour
{
    TextMeshProUGUI stepCountText;

    // Start is called before the first frame update
    void Start()
    {
        Walking.OnStepCountUpdate += TextUpdate;
        stepCountText = GetComponent<TextMeshProUGUI>();
    }

    void TextUpdate(int stepCount)
    {
        Debug.LogFormat("Updated text to: {0}", stepCount);
        stepCountText.text = stepCount.ToString();
    }

    private void OnDestroy()
    {
        Walking.OnStepCountUpdate -= TextUpdate;
    }
}
