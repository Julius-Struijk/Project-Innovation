using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private AudioSource audioSource;

    [SerializeField]
    private AudioClip buttonTap;
    [Range(0f, 1f)]
    public float buttonVolume = 1.0f;

    private void Awake()
    {
        // Singleton pattern: Ensures only one instance exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip, float volume = 1.0f)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip, volume);
        }
    }

    public void ButtonInteractSound()
    {
        audioSource.PlayOneShot(buttonTap, buttonVolume);
    }

}
