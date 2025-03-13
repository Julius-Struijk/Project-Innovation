using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZibbsNoisesScript : MonoBehaviour
{
    //public AudioSource audioSource;
    public AudioClip[] audioClipsZHi;
    public AudioClip[] audioClipsZMi;
    public AudioClip[] audioClipsZLo;
    public AudioClip[] audioClipsZHiMi;
    public AudioClip[] audioClipsZMiLo;
    public AudioClip[] audioClipsZLoMi;
    public AudioClip[] audioClipsZMiHi;
    
    [Range(0f, 1f)]
    public float clipVolume = 1.0f;


    //For functions
    //AudioManager.Instance.PlaySound(audioClips[Random.Range(0, audioClips.Length)], clipVolume);

    public void ZibbsHappyNoise()
    {
        AudioManager.Instance.PlaySound(audioClipsZHi[Random.Range(0, audioClipsZHi.Length)], clipVolume);
    }

    public void ZibbsNeutralNoise()
    {
        AudioManager.Instance.PlaySound(audioClipsZMi[Random.Range(0, audioClipsZMi.Length)], clipVolume);
    }

    public void ZibbsSadNoise()
    {
        AudioManager.Instance.PlaySound(audioClipsZLo[Random.Range(0, audioClipsZLo.Length)], clipVolume);
    }

    public void ZibbsHiMiNoise()
    {
        AudioManager.Instance.PlaySound(audioClipsZHiMi[Random.Range(0, audioClipsZHiMi.Length)], clipVolume);
    }

    public void ZibbsMiLoNoise()
    {
        AudioManager.Instance.PlaySound(audioClipsZMiLo[Random.Range(0, audioClipsZMiLo.Length)], clipVolume);
    }

    public void ZibbsMiHiNoise()
    {
        AudioManager.Instance.PlaySound(audioClipsZMiHi[Random.Range(0, audioClipsZMiHi.Length)], clipVolume);
    }

    public void ZibbsLoMiNoise()
    {
        AudioManager.Instance.PlaySound(audioClipsZLoMi[Random.Range(0, audioClipsZLoMi.Length)], clipVolume);
    }

}
