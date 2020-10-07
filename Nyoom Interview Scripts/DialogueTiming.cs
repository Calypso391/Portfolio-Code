using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueTiming : MonoBehaviour
{
    public List<float> hostDialogueTimings; //list filled with the length of each host line and pause afterwards
    //for example: if the host has two lines, entry 0 is the length of the first line, entry 1 is the length of the first pause, then entry 2 is the length of the second line
    //it assumes that the podcast opens with the host talking
   [HideInInspector] public bool isHostSpeaking = false;
   
   public GameObject reviewMenu; 
   public TextMeshProUGUI scoreText;
    
    ScoreManager sm;

    public void StartTrackHostDialogue(){ //call this once the host starts talking, keeps track of when they are speaking
        StartCoroutine(CoroutineTrackHostDialogue());
        
    }

    IEnumerator CoroutineTrackHostDialogue(){
        int i = 0;
        while(i < hostDialogueTimings.Count){
            isHostSpeaking = !isHostSpeaking;
            yield return new WaitForSeconds(hostDialogueTimings[i]);
            i++;
        }
        yield return new WaitForSeconds(1f);
        GetComponent<AudioRecorder>().isRecording = false;
        ShowReviewMenu();
    }

   void ShowReviewMenu(){
       sm = GetComponent<ScoreManager>();
       reviewMenu.SetActive(true);
       scoreText.text = "Congrats on finishing your interview! Looks like you had a " + (int)sm.score + "% positive rating."; 
       reviewMenu.SetActive(true);
    }
}
