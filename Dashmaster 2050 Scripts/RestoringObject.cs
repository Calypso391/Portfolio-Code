using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RestoringObject : MonoBehaviour
{

    [HideInInspector] public bool triggered = false;
    public KeyCode buttonPress;
    public GameObject Key;
    [HideInInspector] public ScreenRestoreManager.KeyPressData currKeyData;

    public AudioClip triggerSFX;
    AudioSource aSource;

public void OnEnable() {
       aSource = this.gameObject.AddComponent<AudioSource>();
       aSource.playOnAwake = false;
       aSource.loop = false;
       aSource.clip = triggerSFX;
       aSource.volume = .5f;
    }
    public abstract void TriggerObject(); //to be overriden, the behavior for "activating" this object and indicating it can be interacted with
    public abstract void UnTriggerObject();

    public void RestoreScreen( int index, float delay = 0f){ //restores the infinite runner screen, waits by delay amount
        StartCoroutine(doRestoreScreen(delay, index));
    }

    IEnumerator doRestoreScreen(float delay, int index){
        while(delay > 0f){
            delay -= Time.deltaTime;
            yield return null;
        }
        GameObject restoreManager = GameObject.FindWithTag("RestoreManager");
        restoreManager.GetComponent<ScreenRestoreManager>().RestoreScreen(index);
    }

    public void PlayTriggerSFX(){
        aSource.Play();
    }
    public void AddRestoreIndicatorParticles(Vector3 Position){
        
    }
}
