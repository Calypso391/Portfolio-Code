using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class TextWindow : MonoBehaviour
{
    public static float buffer = 0.0f;
    public static float bufferMax = .1f;

    public static GameObject timeControls;
    public static TextMeshProUGUI timePeriod;
    // Start is called before the first frame update
    void Start()
    {
        timeControls = GameObject.Find("Time Controls");
        timePeriod = GameObject.Find("Time Period").GetComponent<TextMeshProUGUI>();
        timeControls.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.windowUp && buffer > 0) buffer -= Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && PlayerController.windowUp && buffer <= 0)
        {
            Debug.Log("Close Window");
            PlayerController.windowUp = false;
            timeControls.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
