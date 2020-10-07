using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioSource bgm;
    public static bool bgmPlaying = false;
    public static AudioSource voice;
    public static bool voiceBusy = false;

    public AudioClip[] robotVoices;
    public static AudioClip[] robotVoiceClips;
    public AudioClip[] effects;
    public static AudioClip[] effectClips;
    // Start is called before the first frame update
    void Start()
    {
        bgm = transform.parent.GetComponent<AudioSource>();
        voice = gameObject.GetComponent<AudioSource>();
        robotVoiceClips = robotVoices;
        effectClips = effects;

        bgmPlaying = false;
        voiceBusy = false;
    }

    public static void StartBGM()
    {
        if (voice == null)
        {
            Debug.Log("No Audio Source attached to player");
            return;
        }
        Debug.Log("BGM Play!");
        bgm.Play();
        bgmPlaying = true;
    }

    public static void EndBGM()
    {
        if (voice == null)
        {
            Debug.Log("No Audio Source attached to player");
            return;
        }
        Debug.Log("BGM Play!");
        bgm.Stop();
        bgmPlaying = false;
    }

    public static void PlayVoice(int index)
    {
        if(voice == null)
        {
            Debug.Log("No Audio Source attached to object");
            return;
        }
        Debug.Log("Voice Play track " + index);
        voice.clip = robotVoiceClips[index - 1];
        if(!voiceBusy) voice.Play();
    }

    public static void PlayEffect(int index)
    {
        if (voice == null)
        {
            Debug.Log("No Audio Source attached to object");
            return;
        }
        Debug.Log("Effect Play track " + index);
        voice.clip = effectClips[index];
        if (!voiceBusy) voice.Play();
    }
}
