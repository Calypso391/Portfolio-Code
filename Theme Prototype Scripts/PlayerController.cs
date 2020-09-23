using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public GameObject breath;
    public GameObject skyDome; // control environment
    public float growSpeed = 0.5f;
    public float maxSize = 10.0f;
    public float fadeSpeed = 0.5f;
    public float fadeTimerMax = 0.5f;
    public float fadeTimer = 0.0f;

    private bool breathing = false;
    private bool endBreath = false;
    public float breathCooldown = 0.5f;
    public float minAccept = 8.0f;
    public float maxAccept = 9.0f;
    public float sizeBuffer = 0.5f;
    public float maxBreatheTime = 1.5f;
    private float breatheTime;

    public Color startColor;
    public Color endColor;
    public float lerpSpeed = 0.2f;
    private Color currColor;
    private float lerpPos = 0;
    private bool canBreathe = true;
    private bool goodBreath = false;
    private Renderer breathRenderer;

    public AudioClip[] sounds;
    private AudioSource audioSource;
    private AudioSource bellSource;
    public float lowBellPitch = 0.3f;
    public float highBellPitch = 0.7f;

    public AudioSource bgm;
    public AudioSource heartbeat;
    public float minBgmPitch = -0.8f;
    public float maxBgmPitch = 1;
    public float pitchMultiplier =  2.0f;

    public Color safeColor = Color.white;
    public Color dangerColor = Color.black;
    public Color overColor = Color.red;
    private Color currLightColor;
    public Light backgroundLight;
    private Renderer rend;
    private float domeTime;
    private float startDomeTime = 0f; //6.66

    public float dangerMaxTime = 2.5f;
    private float dangerTime;

    // Start is called before the first frame update
    void Start()
    {
        fadeTimer = fadeTimerMax;
        audioSource = GetComponent<AudioSource>();
        bellSource = breath.GetComponent<AudioSource>();
        breathRenderer = breath.GetComponent<Renderer>();
        rend = skyDome.GetComponent<Renderer>();
        domeTime = startDomeTime;
        dangerTime = dangerMaxTime;
        breatheTime = maxBreatheTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
        if (Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene(0);
        float closestEnemy = GetClosestEnemy();
        float lerp = 1 / (closestEnemy * pitchMultiplier);
        if (lerp < 0) lerp = 0;
        // skydome
        float offset = domeTime * 0.03f * lerp;
        domeTime += Time.deltaTime;
        if (offset < 0.43){
            rend.material.SetTextureOffset("_MainTex", new Vector2(offset, 0));
        }
        
        bgm.pitch = Mathf.Lerp(maxBgmPitch, minBgmPitch, lerp);
        backgroundLight.color = Color.Lerp(safeColor, dangerColor, lerp);
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) && !breathing && canBreathe)
        {
            breathing = true;
            canBreathe = false;
            goodBreath = false;
            currColor = startColor;
            breath.GetComponent<MeshRenderer>().material.color = startColor;
            //Play Inhale
            audioSource.clip = sounds[0];
            audioSource.Play();
            breathRenderer.material.SetFloat("_AlphaSub", 0);
            breatheTime = maxBreatheTime;
        }
        else if ((Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.Space) || breatheTime <= 0) && breathing)
        {
            endBreath = true;
            lerpPos = 0;
            //Play Exhale
            audioSource.clip = sounds[1];
            audioSource.Play();
        }

        if (breathing)
        {
            if (!endBreath)
            {
                breatheTime -= Time.fixedDeltaTime;
                lerpPos += lerpSpeed * Time.deltaTime;
                currColor = Color.Lerp(startColor, endColor, lerpPos);
                breath.GetComponent<MeshRenderer>().material.color = currColor;

                if (breath.transform.localScale.x < maxSize)
                {
                    Vector3 temp = breath.transform.localScale;
                    temp.x += growSpeed * Time.deltaTime;
                    temp.y += growSpeed * Time.deltaTime;
                    temp.z += growSpeed * Time.deltaTime;
                    breath.transform.localScale = temp;
                }
                if (breath.transform.localScale.x >= minAccept - sizeBuffer && !goodBreath)
                {
                    //Play chime
                    bellSource.pitch = lowBellPitch;
                    bellSource.Play();
                    goodBreath = true;
                }
            }
            else
            {
                float temp = breathRenderer.material.GetFloat("_AlphaSub");
                breathRenderer.material.SetFloat("_AlphaSub", temp + (fadeSpeed * Time.deltaTime));
                fadeTimer -= fadeSpeed * Time.deltaTime;
                if (fadeTimer <= 0.0f)
                {
                    endBreath = false;
                    breathing = false;
                    breath.transform.localScale = Vector3.one;
                    fadeTimer = fadeTimerMax;
                    StartCoroutine(BreathingCooldown());
                }
                // Destroy Enemies on a good release
                if (goodBreath && breath.transform.localScale.x <= maxAccept)
                {
                    Collider[] hitColliders = Physics.OverlapSphere(Vector3.zero, breath.transform.lossyScale.x);
                    int i = 0;
                    while (i < hitColliders.Length)
                    {
                        if (hitColliders[i].gameObject.tag.Equals("Enemy"))
                        {
                            GameObject enemy = hitColliders[i].gameObject;
                            var magnitude = 1000;
                            var force = enemy.transform.position - transform.position;
                            force.Normalize();
                            enemy.GetComponent<Rigidbody>().AddForce(force * magnitude);
                        }
                        i++;
                    }
                    rend.material.SetTextureOffset("_MainTex", new Vector2(0.03f*startDomeTime, 0));
                    domeTime = startDomeTime;
                    dangerTime = dangerMaxTime;
                    closestEnemy = Mathf.Infinity;
                }
            }
            
        }

        if (closestEnemy <= 2.0)
        {
            dangerTime -= Time.deltaTime;
            float endLerp = 1 - (dangerTime / dangerMaxTime);
            heartbeat.volume = Mathf.Lerp(0, 1, endLerp);
            if (dangerTime <= 0)
            {
                SceneManager.LoadScene(0);
            }
            backgroundLight.color = Color.Lerp(dangerColor, overColor, endLerp);
            currLightColor = backgroundLight.color;
        }
        else if(closestEnemy > 2.0 && dangerTime < dangerMaxTime)
        {
            dangerTime += Time.deltaTime;
            if (dangerTime > dangerMaxTime) dangerTime = dangerMaxTime;
            float endLerp = 1 - (dangerTime / dangerMaxTime);
            heartbeat.volume = Mathf.Lerp(0, 1, endLerp);
            backgroundLight.color = Color.Lerp(dangerColor, currLightColor, endLerp);
        }
    }

    IEnumerator BreathingCooldown()
    {
        yield return new WaitForSeconds(breathCooldown);
        //Play chime
        bellSource.pitch = highBellPitch;
        bellSource.Play();
        goodBreath = true;
        canBreathe = true;
    }

    private float GetClosestEnemy()
    {
        Collider[] hitColliders = Physics.OverlapSphere(Vector3.zero, 20);
        if (hitColliders.Length == 0) return -1;

        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach(Collider enemy in hitColliders)
        {
            if (enemy.gameObject.tag.Equals("Enemy"))
            {
                Vector3 directionToTarget = enemy.gameObject.transform.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;

                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                }
            }
                
        }
        return closestDistanceSqr;
    }
}
