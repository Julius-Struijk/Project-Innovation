using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Android;

public class AudioLoudnessDetection : MonoBehaviour
{
    [SerializeField]
    InputHandler inputHandler;

    public int sampleWindow = 64;
    
    AudioClip microphoneClip;
    string microphoneName;

    bool toggleDebug = false;
    //Debug text thingies, assign in inspector
    [SerializeField]
    TextMeshProUGUI loudnessText;
    [SerializeField]
    TextMeshProUGUI nameText;

    // Start is called before the first frame update
    void Start()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Debug.Log("mic permission not authorized");
            Permission.RequestUserPermission(Permission.Microphone);
        }

        

        microphoneName = Microphone.devices[0];
        StartMicRecording();
    }

    // Update is called once per frame
    void Update()
    {
        if (!loudnessText.gameObject.activeSelf && toggleDebug == true)
        {
            loudnessText.gameObject.SetActive(true);
            nameText.gameObject.SetActive(true);
        }

        if (loudnessText.gameObject.activeSelf && toggleDebug == false)
        {
            loudnessText.gameObject.SetActive(false);
            nameText.gameObject.SetActive(false);
        }


        if (inputHandler.toggle.WasPerformedThisFrame())
        {
            if (toggleDebug == true)
            {
                toggleDebug = false;
                return;
            }
            else
            {
                Debug.Log(string.Join(", ", Microphone.devices));
                toggleDebug = true;
            }
        }

        if (loudnessText.gameObject.activeSelf)
        {
            loudnessText.text = "Loudness: " + GetLoudnessFromMicrophone().ToString("F2");
            nameText.text = "Name: " + microphoneName;
        }
    }

    public void StartMicRecording()
    {    
        microphoneClip = Microphone.Start(microphoneName, true, 20, AudioSettings.outputSampleRate);
    }

    public void EndMicRecording()
    {
        Microphone.End(microphoneName);
    }

    public float GetLoudnessFromMicrophone()
    {
        return GetLoudnessFromAudioClip(Microphone.GetPosition(Microphone.devices[0]), microphoneClip);
    }

    //Method to compute the loudness from a certian position in an AudioClip
    public float GetLoudnessFromAudioClip(int clipPosition, AudioClip clip)
    {

        int startPosition = clipPosition - sampleWindow;

        if (startPosition < 0)
        {
            return 0;
        }

        float totalLoudness = 0;
        float[] waveData = new float[sampleWindow];
        clip.GetData(waveData, startPosition);      

        for (int i = 0; i < sampleWindow; i++)
        {
            totalLoudness += Mathf.Abs(waveData[i]);
        }

        return totalLoudness / sampleWindow;
    }
}
