using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{

    [HideInInspector] public float score = 0;
    public float scoreIncrement = 0.4f; //this will need a LOT of tweaking depending on the length of the podcast. may need a seperate increment and decrement value
    public GameObject testSprite;
    public Slider scoreSlider;

    DialogueTiming dialogueTiming;

    public TextMeshProUGUI currentComment;
    public string[] goodComments;
    public string[] badTalkComments;
    public string[] badQuietComments;
    public Color good;
    public Color bad;

    private bool currentCommentPositive = true;
    private float timer = .5f;
    
    private void Start() {
        dialogueTiming = GetComponent<DialogueTiming>();
        dialogueTiming.StartTrackHostDialogue();
    }

    public void OnTalking(){//event triggers when you are talking. increments score if host isnt talking, otherwise decrements
        //testSprite.GetComponent<SpriteRenderer>().color = Color.green;
        testSprite.GetComponent<Image>().color = Color.green;
        if(dialogueTiming.isHostSpeaking){
            score -= scoreIncrement;
            if (currentCommentPositive || timer <= 0)
            {
                currentComment.text = "Most Recent Comment: " + badTalkComments[(int)Random.Range(0, badTalkComments.Length - 1)];
                timer = 3f;
                currentComment.color = bad;
                currentCommentPositive = false;
            }
        } else{
            score += scoreIncrement;
            if (!currentCommentPositive || timer <= 0)
            {
                currentComment.text = "Most Recent Comment: " + goodComments[(int)Random.Range(0, badTalkComments.Length - 1)];
                timer = 3f;
                currentComment.color = good;
                currentCommentPositive = true;
            }
        }   
    }

    public void OnNoTalking(){ //event triggers when you are not talking. increments score if the host is talking, otherwise decrements
        //testSprite.GetComponent<SpriteRenderer>().color = Color.red;
        testSprite.GetComponent<Image>().color = Color.red;
        if(dialogueTiming.isHostSpeaking){
            score += scoreIncrement;
            if (!currentCommentPositive || timer <= 0)
            {
                currentComment.text = "Most Recent Comment: " + goodComments[(int)Random.Range(0, badTalkComments.Length - 1)];
                timer = 3f;
                currentComment.color = good;
                currentCommentPositive = true;
            }
        } else{
            score -= scoreIncrement;
            if (currentCommentPositive || timer <= 0)
            {
                currentComment.text = "Most Recent Comment: " + badQuietComments[(int)Random.Range(0, badTalkComments.Length - 1)];
                timer = 3f;
                currentComment.color = bad;
                currentCommentPositive = false;
            }
        }
    }

    private void Update() { //clamping score between 100f and 0f
        if(score >= 100){
            score = 100f;
        } else if(score <= 0){
            score = 0f;
        }
        scoreSlider.value = score;
        if (timer > 0) timer -= Time.fixedDeltaTime;
    }

    
}
