using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform[] travelNodes;
    public int positionInt = 0;

    public static float bufferMax = .2f;
    private float bufferTimer = 0.0f;
    private bool buffering = false;

    public bool isActive;
    public PlayerController otherPlayer;

    public static bool windowUp = false;
    public static bool started = false;

    public AudioSource playerAudio;

    // Start is called before the first frame update
    void Start()
    {
        //positionInt = 0;
        //Vector3 translation = travelNodes[0].position;
        //translation.y = transform.position.y;
        //transform.position = translation;

        if (!started)
        {
            windowUp = true;
            started = true;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (buffering)
        {
            bufferTimer += Time.fixedDeltaTime;
            if(bufferTimer >= bufferMax)
            {
                buffering = false;
                bufferTimer = 0.0f;
            }
        }
        if (!windowUp)
        {
            if (Input.GetKeyDown("space"))
            {
                // setting the other player to my position and rotation
                otherPlayer.GetComponent<PlayerController>().positionInt = positionInt;
                Vector3 translation = otherPlayer.GetComponent<PlayerController>().travelNodes[positionInt].position;
                translation.y = transform.position.y;
                otherPlayer.transform.position = translation;
                otherPlayer.transform.rotation = gameObject.transform.rotation;

                otherPlayer.gameObject.SetActive(true);
                otherPlayer.transform.GetChild(0).gameObject.SetActive(true);
                gameObject.SetActive(false);
                transform.GetChild(0).gameObject.SetActive(false);

                if (TextWindow.timePeriod.text.Equals("Present")) TextWindow.timePeriod.text = "Past";
                else TextWindow.timePeriod.text = "Present";
                return;
            }
            if (!buffering)
            {
                if (Input.GetAxisRaw("Horizontal") != 0)
                {
                    //Rotation
                    int rotation = 0;
                    rotation = Input.GetAxisRaw("Horizontal") == 1 ? 90 : -90;
                    transform.Rotate(0, rotation, 0);
                    buffering = true;
                    return;
                }
                if (Input.GetAxisRaw("Vertical") == 1)
                {
                    float tempY = transform.rotation.eulerAngles.y;
                    if ((positionInt == 0 && (tempY == 0))
                        || (positionInt == 1 && (tempY == 90 || tempY == -270))
                        || (positionInt == 2 && (tempY == -180 || tempY == 180)))
                    {
                        positionInt++;
                        playerAudio.pitch = Random.Range(.7f, 1);
                        playerAudio.Play();
                    }
                    else if (((positionInt == 3) && (tempY == 0))
                        || (positionInt == 1 && (tempY == 180 || tempY == -180))
                        || (positionInt == 2 && (tempY == -90 || tempY == 270)))
                    {
                        positionInt--;
                        playerAudio.pitch = Random.Range(.7f, 1);
                        playerAudio.Play();
                    }
                    buffering = true;
                    Vector3 translation = travelNodes[positionInt].position;
                    translation.y = transform.position.y;
                    transform.position = translation;
                }
            }
        }
    }
}
