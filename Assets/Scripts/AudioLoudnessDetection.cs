using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Android;

public class AudioLoudnessDetection : MonoBehaviour
{
    //Reference to the Input Handler
    [SerializeField]
    InputHandler inputHandler;

    //The length of the computed audio sample
    public int sampleWindow = 64;

    //The audio clip recorded by the microphone
    AudioClip microphoneClip;

    //The name of the current microphone device
    string microphoneName;

    //Bool to show debug text or not
    bool toggleDebug = false;
    //Debug text thingies, assign in inspector
    [SerializeField]
    TextMeshProUGUI loudnessText;
    [SerializeField]
    TextMeshProUGUI nameText;
    [SerializeField]
    TextMeshProUGUI capsText;

   
    void Start()
    {
        //Get the current connected mic
        microphoneName = Microphone.devices[0];
    }

    
    void Update()
    {
        //Some logic for the toggling of the debug text
        if (loudnessText != null && !loudnessText.gameObject.activeSelf && toggleDebug == true)
        {
            loudnessText.gameObject.SetActive(true);
            nameText.gameObject.SetActive(true);
            capsText.gameObject.SetActive(true);
        }

        if (loudnessText != null && loudnessText.gameObject.activeSelf && toggleDebug == false)
        {
            loudnessText.gameObject.SetActive(false);
            nameText.gameObject.SetActive(false);
            capsText.gameObject.SetActive(false);
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

        if (loudnessText != null && loudnessText.gameObject.activeSelf)
        {
            int minFreq;
            int maxFreq;
            Microphone.GetDeviceCaps(Microphone.devices[0], out minFreq, out maxFreq);
            loudnessText.text = "Loudness: " + GetLoudnessFromMicrophone().ToString("F2");
            nameText.text = "Name: " + microphoneName;
            capsText.text = "Time: " + minFreq + ", " + maxFreq;
        }
    }

    //Methods to stop and start the mic recording
    public void StartMicRecording()
    {
        //Make sure the device has permitted the app to use the microphone
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Debug.Log("mic permission not authorized");
            Permission.RequestUserPermission(Permission.Microphone);
        }

        microphoneClip = Microphone.Start(microphoneName, true, 20, AudioSettings.outputSampleRate);
    }

    public void EndMicRecording()
    {
        Microphone.End(microphoneName);
    }

    //Method to returns the loudness of the audio provided by the microphone
    public float GetLoudnessFromMicrophone()
    {
        return GetLoudnessFromAudioClip(Microphone.GetPosition(Microphone.devices[0]), microphoneClip);
    }

    //Method to compute the loudness from a certian position in an AudioClip
    public float GetLoudnessFromAudioClip(int clipPosition, AudioClip clip)
    {
        //Get the start of the sample
        int startPosition = clipPosition - sampleWindow;

        //Ignore it if the start is outside of the clip
        if (startPosition < 0)
        {
            return 0;
        }

        float totalLoudness = 0;
        float[] waveData = new float[sampleWindow];
        //Get the data from the entire samplewindow and add it to the total loudness
        clip.GetData(waveData, startPosition);            
        for (int i = 0; i < sampleWindow; i++)
        {
            totalLoudness += Mathf.Abs(waveData[i]);
        }

        return (totalLoudness / sampleWindow) * 50f;
    }
}
