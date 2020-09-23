using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OtherInputController : MonoBehaviour
{
    private GameObject intro;
    // Start is called before the first frame update
    void Start()
    {
        intro = GameObject.Find("Canvas");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
        if ((Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.Space)) && intro.activeSelf) intro.SetActive(false);
    }

    public void ToCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void ToMain()
    {
        SceneManager.LoadScene("CalistaTest");
    }
}
