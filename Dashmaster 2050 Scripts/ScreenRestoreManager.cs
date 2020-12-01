using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScreenRestoreManager : MonoBehaviour
{
    //NOTE:
    //The restoring objects must be entered in right to left. 
    //availible keys to press must be entered in the same order as the key sprites and animations 
    //Example: up arrow in spot zero corresponds to up arrow sprite and up arrow anim in spot zero in the other lists)
    
    public string NOTE = "see script for ordering of public fields";
    public List<GameObject> restoringObjects = new List<GameObject>();
    public GameObject levelCamera;
    public GameObject motivationMeter;

    public List<KeyCode> possibleKeys = new List<KeyCode>();
    public List<Sprite> keySprites = new List<Sprite>();
    public List<RuntimeAnimatorController> keyAnimatorControllers = new List<RuntimeAnimatorController>();
     Stack<GameObject> toUnTrigger = new Stack<GameObject>();
    LinkedList<GameObject> toTrigger = new LinkedList<GameObject>();
    CameraController cc;

    [HideInInspector] public float motivation = 1;
    public float motivationDecreaseAmount = .1f;
    bool onEndScreen = false;

    List<KeyPressData> runtimeKeyData = new List<KeyPressData>();
    public GameObject endScreen;
    public GameObject backgroundAudioObj;
    public GameObject virtualPlayerObj;
    public AudioClip restoreSound;

    public AudioClip failSound;

    // Start is called before the first frame update
    void Start()
    {
        cc = levelCamera.GetComponent<CameraController>();
        for(int i = 0; i < restoringObjects.Count; i++){
            toTrigger.AddLast(restoringObjects[i]);
        }

        for(int i = 0; i < possibleKeys.Count; i++){
            KeyPressData newKey = new KeyPressData();
            newKey.code = possibleKeys[i];
            newKey.anim = keyAnimatorControllers[i];
            newKey.sprite = keySprites[i];
            runtimeKeyData.Add(newKey);
        }

    }

/*
    public void TriggerRandomRestorableObject(){ //triggers a random object that can restore screen space
        int i = Random.Range(0, restoringObjects.Count);
        restoringObjects[i].GetComponent<RestoringObject>().TriggerObject();
    }

    public void TriggerRestorableObject(int index)
    {
        restoringObjects[index].GetComponent<RestoringObject>().TriggerObject();
    }
*/
    public void TriggerNextObject(){

        if(toTrigger.Count == 0){
            Debug.Log("tried to trigger when no objects are left");
            return;
        }
        GameObject o = toTrigger.First.Value;
        toTrigger.RemoveFirst();
        RestoringObject ro = o.GetComponent<RestoringObject>();
        ro.TriggerObject();
        ro.PlayTriggerSFX();
        SetKey(ro);
        toUnTrigger.Push(o);
        Debug.Log("triggered with "  + o.GetComponent<RestoringObject>().buttonPress);
    }


    public void UnTriggerNextObject(){
        GameObject o = toUnTrigger.Pop();
        RestoringObject ro = o.GetComponent<RestoringObject>();
        ro.UnTriggerObject();
        ro.Key.SetActive(false);
       runtimeKeyData.Add(ro.currKeyData);
        toTrigger.AddFirst(o);
    }
    public void RestoreScreen(int index){
        //ToggleMotivation(false);
        this.GetComponent<AudioSource>().clip = restoreSound;

        this.GetComponent<AudioSource>().Play();
        cc.ChangeRange(index);
    }

    void SetKey(RestoringObject ro){
        ro.Key.SetActive(true);
        int i = Random.Range(0, runtimeKeyData.Count);
        ro.buttonPress = runtimeKeyData[i].code;
        ro.Key.GetComponent<SpriteRenderer>().sprite = runtimeKeyData[i].sprite;
        ro.Key.GetComponent<Animator>().runtimeAnimatorController = runtimeKeyData[i].anim;
        ro.currKeyData = runtimeKeyData[i];
        runtimeKeyData.Remove(runtimeKeyData[i]);  
    }


    /*
    public void ToggleMotivation(bool enabledOrDisabled){
        if(motivationMeter.activeInHierarchy && enabledOrDisabled == false){
            motivation = 1;
            motivationDecreaseAmount *= 1.3f;
            StopCoroutine("doMotivationDecrease");
            motivationMeter.transform.parent.gameObject.SetActive(false);

        } else if(!motivationMeter.activeInHierarchy && enabledOrDisabled == true){
            StartCoroutine("doMotivationDecrease");
        }
    }

    IEnumerator doMotivationDecrease(){
        motivationMeter.transform.parent.gameObject.SetActive(true);
        while(true){
            motivationMeter.GetComponent<Image>().fillAmount = motivation;
            motivation -= motivationDecreaseAmount * Time.deltaTime;

            if(motivation <= 0){
                 
                 onEndScreen = true;
                endScreen.SetActive(true);
                break;
            } 
            yield return null;
        }
    }
*/
    private void Update() {
        if(onEndScreen){
            if(Input.GetKeyDown(KeyCode.Space)){
                SceneManager.LoadScene(0);
            }
        }

        if(toUnTrigger.Count != 0){
            if(Input.GetKeyDown(toUnTrigger.Peek().GetComponent<RestoringObject>().buttonPress)){ //this is not terribly efficient :(
            UnTriggerNextObject();
        }
        }
        
    }

    public void SetEndScreen(){ //TODO: fail sfx
        onEndScreen = true;
                endScreen.SetActive(true);
                backgroundAudioObj.GetComponent<AudioSource>().Stop();
                virtualPlayerObj.GetComponent<AudioSource>().Stop();
                this.GetComponent<AudioSource>().clip = failSound;
        this.GetComponent<AudioSource>().volume = .5f;
        this.GetComponent<AudioSource>().Play();
    }


    public struct KeyPressData{
        public KeyCode code;
        public RuntimeAnimatorController anim;
        public Sprite sprite;
    }
}
