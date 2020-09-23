using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickKiller : MonoBehaviour
{

    public Text answer;
    public AudioSource correctAudio;
    public AudioSource wrongAudio;
    public AudioSource envAudio;
    private bool alreadyPicked = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChoseCorrect()
    {
        if (!alreadyPicked)
        {
            answer.text = "Correct! Mary was the murderer.";
            //envAudio.Stop();
            correctAudio.Play();
            alreadyPicked = true;
        }
    }

    public void ChoseWrong()
    {
        if(!alreadyPicked)
        {
            answer.text = "Wrong! Lucky for you rewinding time is as easy as hitting R.";
            //envAudio.Stop();
            wrongAudio.Play();
            alreadyPicked = true;
        }
    }

    //void OnMouseDown()
    //{
    //    if (gameObject.tag == "Killer")
    //    {
    //        answer.text = "Correct! Maid 1 was the murderer.";
    //    }
    //    else
    //    {
    //        answer.text = "Wrong! Maid 1 was the murderer.";
    //    }
    //}

    //void OnMouseOver()
    //{
    //    //If your mouse hovers over the GameObject with the script attached, output this message
    //    Debug.Log("Mouse is over GameObject.");
    //}

    //void OnMouseExit()
    //{
    //    //The mouse is no longer hovering over the GameObject so output this message each frame
    //    Debug.Log("Mouse is no longer on GameObject.");
    //}

}
