using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] audioClips;
    public AudioSource subManager;
    public AudioClip[] subClips;

    public void PlayClip(int index)
    {
        audioSource.clip = audioClips[index];
        audioSource.Play();
    }

    public void PlaySubClip(int index)
    {
        subManager.clip = subClips[index];
        subManager.Play();
    }
}
