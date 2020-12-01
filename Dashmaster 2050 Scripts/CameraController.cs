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
    public static GameObject controls;
    private TextMeshProUGUI scoreText;
    private float score = 0;

    public GameObject screenRestoreManager;
    public GameObject infiniteRunnerGenerator;

    public GameObject shredder;

    public GameObject CalorieParticle1;
    public GameObject CalorieParticle2;

    // Start is called before the first frame update
    void Start()
    {
        cameraY = transform.position.y;
        cameraZ = transform.position.z;

        ChangeRange(0);

        controls = GameObject.Find("Controls");
        controls.SetActive(true);

        scoreText = GameObject.Find("Score").GetComponent<TextMeshProUGUI>();
        dead = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
        if (Input.GetKeyDown(KeyCode.R))
        {
            ChangeRange();
            return;
        }
        //if (Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene(0);


        bufferTimer -= Time.deltaTime;

        //Reset to beginning position
        //ChangeRange();

        if (!dead)
        {
            score += Time.fixedDeltaTime * (InfiniteRunnerGenerator.Instance.currentComboLevel + 1);
            score = Mathf.Round(score * 100f) / 100f;
            scoreText.text = "Calories Burned: " + (int)score;
        }

        if (InfiniteRunnerGenerator.Instance.currentComboLevel == 0)
        {
            CalorieParticle1.SetActive(false);
            CalorieParticle2.SetActive(false);

        }
        if (InfiniteRunnerGenerator.Instance.currentComboLevel == 1)
        {
            CalorieParticle1.SetActive(true);
            CalorieParticle2.SetActive(false);

        }
        if (InfiniteRunnerGenerator.Instance.currentComboLevel == 2)
        {
           CalorieParticle1.SetActive(false);
            CalorieParticle2.SetActive(true);
       }
    }

    public void ChangeRange(int change = int.MinValue)
    {
        if (bufferTimer > 0) return;

        bufferTimer = .25f;
        index += change;

        if (index < 0) index = 0;
        else if (index >= cameraPos.Length) index = cameraPos.Length - 1;

        transform.position = new Vector3(cameraPos[index], cameraY, cameraZ);
        subCamera.rect = new Rect(0, 0, viewportSize[index], 1);
        ScreenShake.instance.TriggerShake(.1f, 0);
        transform.position = new Vector3(cameraPos[index], cameraY, cameraZ);

        /*if(index == 2){ //activate random restorable event after second hit
            screenRestoreManager.GetComponent<ScreenRestoreManager>().TriggerRandomRestorableObject();
        }*/
        if(change == 1 && !dead) screenRestoreManager.GetComponent<ScreenRestoreManager>().TriggerNextObject();
        if (index == cameraPos.Length - 1 && !dead)
        {
            //screenRestoreManager.GetComponent<ScreenRestoreManager>().ToggleMotivation(true);
            //shredder.transform.position = new Vector3(51.92f, 0, -1);
            dead = true; //TODO: transition from dead screen to end screen
            StartCoroutine(setEndScreenWithDelay());
            return;
        }
        ScreenShake.instance.TriggerShake(.1f, 0);
    }

    IEnumerator setEndScreenWithDelay(float delay = 1){
        while(delay > 0){
            delay -= Time.deltaTime;
            yield return null;
        }
        screenRestoreManager.GetComponent<ScreenRestoreManager>().SetEndScreen();
    }

}
