using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Timer : MonoBehaviour
{
    public float timeRemaining = 60;
    public bool timerIsRunning = false;
    public TextMeshProUGUI timeText;

    public AudioSource timerAudio;
    public AudioSource policeAudio;

    private void Start()
    {
        // Starts the timer automatically
        timerIsRunning = true;
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                timerIsRunning = false;
                SceneManager.LoadScene("Identify_Killer");
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        if (minutes >= 1)
        {
            timerAudio.volume = 0;
            policeAudio.volume = 0;
        }
        else
        {
            timerAudio.volume = Mathf.Lerp(0, 1, (60 - seconds) / 60);
            if(seconds <= 30) policeAudio.volume = Mathf.Lerp(0, .5f, (30 - seconds) / 30);
        }

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}