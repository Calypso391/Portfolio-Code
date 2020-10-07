using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OtherControls : MonoBehaviour
{
    public static GameObject gameOverText;
    // Start is called before the first frame update
    void Start()
    {
        gameOverText = GameObject.Find("RToRestart");
        gameOverText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
        if (Input.GetKeyDown(KeyCode.R)) ReloadGame();
    }

    public static IEnumerator GameOver()
    {
        GameObject.Find("WallSpawner").SetActive(false);
        GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
        foreach(GameObject wall in walls)
        {
            Destroy(wall);
        }
        AudioManager.PlayEffect(0);
        AudioManager.EndBGM();
        yield return new WaitForSeconds(.5f);
        AudioManager.PlayEffect(1);
        gameOverText.SetActive(true);
    }

    public static void ReloadGame()
    {
        SceneManager.LoadScene("Calista Test");
    }
}
