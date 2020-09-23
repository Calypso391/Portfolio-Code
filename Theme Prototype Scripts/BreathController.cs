using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathController : MonoBehaviour
{
    private static AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public static void PlayBell()
    {
        audioSource.Play();
    }
}
