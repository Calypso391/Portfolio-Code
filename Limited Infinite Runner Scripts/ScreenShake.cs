using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
  public static ScreenShake instance;

    // Desired duration of the shake effect
    public float shakeDuration = 0f;

    float currentDur = 0f;

    // A measure of magnitude for the shake. Tweak based on your preference
    public float shakeMagnitude = 0.7f;

    // The initial position of the GameObject
    Vector3 initialPosition;

    private void Awake()
    {
        instance = this;
    }

    void OnEnable()
    {
    }

    private void FixedUpdate()
    {
    }

    public void TriggerShake()
    {
        currentDur = shakeDuration;
    }

    public void TriggerShake(float dur, float pIntensity)
    {
        initialPosition = transform.localPosition;
        currentDur = dur;
        shakeMagnitude = pIntensity;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentDur > 0)
        {
            transform.localPosition = initialPosition + (Vector3)Random.insideUnitCircle * shakeMagnitude;
            currentDur -= Time.deltaTime;
        }
        else
        {

            if(initialPosition != Vector3.zero)
            transform.localPosition = initialPosition;
            currentDur = 0f;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TriggerShake();
        }

    }
}
