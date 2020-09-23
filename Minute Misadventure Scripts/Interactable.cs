using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Interactable : MonoBehaviour
{
    public static GameObject textWindow;
    public static TextMeshProUGUI nameText;
    public static TextMeshProUGUI interactText;
    public static TextMeshProUGUI cluesText;

    public string objectName = "";
    public string[] interactTexts;
    public bool[] isClue;
    public bool[] alreadyRead;
    private int interactIndex = 0;
    private int indexMax = 1;

    public bool special = false;
    private bool specialTrigger = false;
    public Interactable triggerable;

    public static int currentClues = 0;
    public AudioSource interactSound;
    public AudioClip audioClip;

    // Start is called before the first frame update
    void Start()
    {
        textWindow = GameObject.Find("Canvas");
        nameText = GameObject.Find("Name").GetComponent<TextMeshProUGUI>();
        interactText = GameObject.Find("Text").GetComponent<TextMeshProUGUI>();
        cluesText = GameObject.Find("Clues").GetComponent<TextMeshProUGUI>();

        indexMax = interactTexts.Length;
    }

    void OnMouseDown()
    {
        if (!PlayerController.windowUp)
        {
            Debug.Log("Open Window " + objectName + ": " + interactIndex);
            //Show text window
            textWindow.SetActive(true);
            TextWindow.timeControls.SetActive(false);
            TextWindow.buffer = TextWindow.bufferMax;

            interactSound.clip = audioClip;
            interactSound.Play();

            if (objectName == "") nameText.transform.parent.gameObject.SetActive(false);
            else nameText.transform.parent.gameObject.SetActive(true);

            nameText.text = objectName;
            interactText.text = interactTexts[interactIndex];
            PlayerController.windowUp = true;

            if(!alreadyRead[interactIndex] && isClue[interactIndex])
            {
                alreadyRead[interactIndex] = true;
                currentClues++;
                cluesText.text = "Clues: " + currentClues + "/18";
            }

            //Interate text for next click
            if (triggerable != null && (!special || (special && specialTrigger)))
            {
                triggerable.specialTrigger = true;
                triggerable.interactIndex++;
                Destroy(gameObject);
            }
            if (interactIndex < indexMax - 1 && (!special || (special && specialTrigger))) interactIndex++;
        }
    }
}
