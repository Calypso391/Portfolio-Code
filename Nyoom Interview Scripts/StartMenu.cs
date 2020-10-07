using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public GameObject pauseUI;
    public VideoPlayer videoPlayer;
    public AudioSource audioPlayer;
    // Start is called before the first frame update
    void Start()
    {
        Pause();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Quit();
        if (Input.GetKeyDown(KeyCode.R)) Restart();
        if (Input.GetMouseButtonDown(0)) audioPlayer.Play();
    }

    public void Pause()
    {
        pauseUI.SetActive(true);
        videoPlayer.Pause();
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        pauseUI.SetActive(false);
        videoPlayer.Play();
        Time.timeScale = 1f;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Restart()
    {
        SceneManager.LoadScene("Calista Test");
    }
}
