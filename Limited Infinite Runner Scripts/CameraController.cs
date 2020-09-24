using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CameraController : MonoBehaviour
{
    public float[] cameraPos;
    private float cameraY;
    private float cameraZ;
    public float[] viewportSize;
    private int index = 0;

    public Camera subCamera;

    private float bufferTimer = 0;

    public static bool dead = false;
    private GameObject tryAgain;
    public static GameObject controls;
    private TextMeshProUGUI scoreText;
    private float score = 0;

    // Start is called before the first frame update
    void Start()
    {
        cameraY = transform.position.y;
        cameraZ = transform.position.z;

        ChangeRange(0);

        tryAgain = GameObject.Find("Try Again");
        tryAgain.SetActive(false);

        controls = GameObject.Find("Controls");
        controls.SetActive(true);

        scoreText = GameObject.Find("Score").GetComponent<TextMeshProUGUI>();
        dead = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
        if (Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene("Calista Test");

        bufferTimer -= Time.deltaTime;

        if (!dead)
        {
            score += Time.fixedDeltaTime;
            score = Mathf.Round(score * 100f) / 100f;
            scoreText.text = "Score: " + score;
        }
    }

    public void ChangeRange(int change = -1)
    {
        if (bufferTimer > 0) return;

        bufferTimer = .25f;
        index += change;

        if (index < 0) index = 0;
        else if (index >= cameraPos.Length) index = cameraPos.Length - 1;
            
        transform.position = new Vector3(cameraPos[index], cameraY, cameraZ);
        subCamera.rect = new Rect(0, 0, viewportSize[index], 1);


        if (index == cameraPos.Length - 1)
        {
            dead = true;
            tryAgain.SetActive(true);
            return;
        }
    }
}
