using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public KeyCode[] keys;
    public Dictionary<KeyCode, int> bindings = new Dictionary<KeyCode, int>(); //Used for keypresses
    public EColorFlag[] colors;
    public EColorFlag playerColor;
    public List<AudioClip> colorChangeSounds = null;

    private float cooldown = 0;
    public float cooldownMax = .3f;

    private Renderer playerRenderer;

    public int level = 0;

    public List<Button> buttons = new List<Button>();
    private int prevSelected = -1;

    //VFXs
    private float effectTimer = 0.0f;
    public float effectTimerMax = .5f;
    private Vector3 localScale;
    public float successFrequency = 20f;
    public float failFrequency = 20f;
    public float successMagnitude = 2f;
    public float failMagnitude = .5f;

    public float buttonEffectTimerMax = 2f;
    public float buttonFrequency = 10f;
    public float buttonMagnitude = 1.5f;

    private Slider health;
    private Image miniPlayer;
    private CameraShake camerashake;


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 9; i++)
        {
            bindings.Add(keys[i], i);
        }
        playerRenderer = gameObject.GetComponent<Renderer>();
        health = GameObject.Find("Slider").GetComponent<Slider>();
        miniPlayer = health.transform.GetChild(2).GetChild(0).gameObject.GetComponent<Image>();
        camerashake = GameObject.Find("Main Camera").GetComponent<CameraShake>();

        localScale = transform.localScale;
        ShuffleBindings(9);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && buttons[0].gameObject.activeSelf) SetNewColor(KeyCode.Q, 0);
        else if (Input.GetKeyDown(KeyCode.W) && buttons[1].gameObject.activeSelf) SetNewColor(KeyCode.W, 1);
        else if (Input.GetKeyDown(KeyCode.E) && buttons[2].gameObject.activeSelf) SetNewColor(KeyCode.E, 2);
        else if (Input.GetKeyDown(KeyCode.A) && buttons[3].gameObject.activeSelf) SetNewColor(KeyCode.A, 3);
        else if (Input.GetKeyDown(KeyCode.S) && buttons[4].gameObject.activeSelf) SetNewColor(KeyCode.S, 4);
        else if (Input.GetKeyDown(KeyCode.D) && buttons[5].gameObject.activeSelf) SetNewColor(KeyCode.D, 5);
        else if (Input.GetKeyDown(KeyCode.Z) && buttons[6].gameObject.activeSelf) SetNewColor(KeyCode.Z, 6);
        else if (Input.GetKeyDown(KeyCode.X) && buttons[7].gameObject.activeSelf) SetNewColor(KeyCode.X, 7);
        else if (Input.GetKeyDown(KeyCode.C) && buttons[8].gameObject.activeSelf) SetNewColor(KeyCode.C, 8);

    }

    public void ShuffleBindings(int totalAllowed)
    { //Goes through and rebinds 0-11, None is left at element 12
        for (int i = 11; i > 0; i--)
        {
            int ran = Random.Range(0, i);
            EColorFlag temp = colors[i];
            colors[i] = colors[ran];
            colors[ran] = temp;
        }

        StartCoroutine(RevealKeys(totalAllowed));

        if (prevSelected > -1 && prevSelected < 9) buttons[prevSelected].gameObject.transform.GetChild(0).GetComponent<Text>().color = Color.black;

        playerColor = EColorFlag.None;
        playerRenderer.material.SetColor("_Color", EColorRGBHelper.FromSingleFlagToRGB(playerColor));
        miniPlayer.color = EColorRGBHelper.FromSingleFlagToRGB(playerColor);
    }

    public void SetNewColor(KeyCode key, int button)
    {
        AudioSource.PlayClipAtPoint(colorChangeSounds[Random.Range(0, colorChangeSounds.Count - 1)], new Vector3(0.0f, 0.0f - 10.0f));
        EColorFlag tempColor = colors[bindings[key]];
        playerRenderer.material.SetColor("_Color", EColorRGBHelper.FromSingleFlagToRGB(tempColor));
        miniPlayer.color = EColorRGBHelper.FromSingleFlagToRGB(tempColor);
        playerColor = tempColor;
        cooldown = cooldownMax;
        buttons[button].image.color = EColorRGBHelper.FromSingleFlagToRGB(tempColor);

        StartCoroutine(ButtonFade(key, button));

        if (prevSelected != button)
        {
            if (prevSelected > -1 && prevSelected < 9) buttons[prevSelected].gameObject.transform.GetChild(0).GetComponent<Text>().color = Color.black;
            buttons[button].gameObject.transform.GetChild(0).GetComponent<Text>().color = Color.white;
            prevSelected = button;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Wall hitWall = collision.gameObject.GetComponent<Wall>();
        if (hitWall.color == playerColor)
        {
            hitWall.Clear();
            StartCoroutine(SuccessHit());
            if (health.value < 10) health.value++;
        }
        else
        {
            hitWall.Fail();
            StartCoroutine(FailHit());
            if (health.value > 0) health.value--;
            if (health.value == 0) StartCoroutine(OtherControls.GameOver());
        }
    }

    private IEnumerator SuccessHit()
    {
        effectTimer = 0.0f;
        transform.localScale = localScale;

        while (effectTimer < effectTimerMax)
        {
            effectTimer += Time.deltaTime;
            Vector3 tempScale = transform.localScale;
            float newScale = Mathf.Sin(Time.time * successFrequency) * successMagnitude;
            tempScale.x = newScale;
            tempScale.y = newScale;
            transform.localScale = tempScale;
            yield return null;
        }

        transform.localScale = localScale;
    }

    private IEnumerator FailHit()
    {
        effectTimer = 0.0f;
        transform.localScale = localScale;
        StartCoroutine(camerashake.Shake(.3f, Random.Range(.15f, .3f)));

        while (effectTimer < effectTimerMax)
        {
            effectTimer += Time.deltaTime;
            Vector3 tempScale = transform.localScale;
            float newScale = Mathf.Sin(Time.time * failFrequency) * failMagnitude;
            tempScale.x = newScale;
            tempScale.y = newScale;
            transform.localScale = tempScale;
            yield return null;
        }

        transform.localScale = localScale;
    }

    private IEnumerator ButtonFade(KeyCode key, int button)
    {
        float buttonEffectTimer = 0.0f;
        Vector3 buttonScale = buttons[button].transform.localScale;

        Color startColor = EColorRGBHelper.FromSingleFlagToRGB(colors[button]);
        while (buttonEffectTimer < buttonEffectTimerMax)
        {
            if (Input.GetKeyDown(key) && buttonEffectTimer > .1f) yield break;
            buttonEffectTimer += Time.deltaTime;
            Color tempColor = Color.Lerp(startColor, Color.gray, buttonEffectTimer / buttonEffectTimerMax);
            buttons[button].image.color = tempColor;

            yield return null;
        }

        buttons[button].transform.localScale = buttonScale;
    }

    private IEnumerator ButtonHint(int allowed)
    {
        buttonEffectTimerMax = 2.5f;
        for (int i = 0; i < allowed; i++)
        {

            StartCoroutine(ButtonFade(keys[i], i));
        }
        yield return new WaitForSeconds(2.5f);
        buttonEffectTimerMax = 1.5f;
    }

    private IEnumerator RevealKeys(int totalAllowed)
    {
        if (totalAllowed < 9 || (totalAllowed == 9 && !buttons[8].gameObject.activeSelf))
        for (int j = 0; j < 9; j++)
        {
            buttons[j].image.color = Color.grey;
            if (j < totalAllowed)
            {
                buttons[j].gameObject.SetActive(true);
                yield return new WaitForSeconds(.15f);
            }
            else
            {
                buttons[j].gameObject.SetActive(false);
            }   
        }
        StartCoroutine(ButtonHint(totalAllowed));
    }

}
