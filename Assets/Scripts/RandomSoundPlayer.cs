using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSoundPlayer : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] audioClips;
    [Range(0f, 1f)]
    public float clipVolume = 1.0f;



    //On _ Trigger:
    ////choose random clip
    //AudioClip randomClip = audioClips[Random.Range(0, audioClips.Length)];

    ////play audio clip
    //audioSource.PlayOneShot(randomClip, clipVolume);
}
