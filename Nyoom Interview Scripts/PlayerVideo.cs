using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerVideo : MonoBehaviour
{

    static WebCamTexture playerCam;
    public GameObject videoOff; //the video off graphic

    [HideInInspector] public bool camOn = false;

    // Start is called before the first frame update
    void Start()
    {
        if(playerCam != null){
            playerCam.Stop();
            playerCam = null;
        }
        if(WebCamTexture.devices.Length == 0){
            playerCam = null;
            Debug.Log("no camera");
            return;
        }
        
         playerCam = new WebCamTexture();
         GetComponent<Image>().material.mainTexture = playerCam;
      //  playerCam.Play();
    }

    public void StartCamera(){
        camOn = true;
     //   playerCam = new WebCamTexture();
      //   GetComponent<Image>().material.mainTexture = playerCam;
      playerCam.Play();
       videoOff.SetActive(false);
    }

    public void StopCamera(){
        camOn = false;
        playerCam.Stop();
        videoOff.SetActive(true);
    }

    public void SwitchWebcam(){

        if(playerCam == null){
            videoOff.SetActive(true);
            return;
        }

        if(!camOn){
            StartCamera();
        } else{
            StopCamera();
        }
    }

    public void RestartCam(){
         camOn = true;
    
      playerCam.Play();
       videoOff.SetActive(false);
    }

    
}
