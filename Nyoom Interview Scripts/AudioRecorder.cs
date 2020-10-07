using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioRecorder : MonoBehaviour
{
    
    public AudioSource recordingAudioSource; //audio source that processes the recording
    public AudioSource playbackAudioSource; //audio source that will play back the recoridng
    public AudioMixerGroup microphoneGroup; //the microphone audio group 

    AudioClip recordingClip;
    ScoreManager scoreManager; //handles adjusting score and any other visual feedback
   public bool armRecording = false; //primes for recording

   [HideInInspector] public bool isRecording = false;

   public float clipLoudnessThreshold = 0.001f; //how loud should our input volume be to register as talking?


//variables for processing clip loudness
    public float updateStep = 0.2f;
     public int sampleDataLength = 1024;
 
     private float currentUpdateTime = 0f;
 
     private float clipLoudness;
     private float[] clipSampleData;

    bool isMuted = false;

    // Start is called before the first frame update
    void Start()
    {
       recordingAudioSource.outputAudioMixerGroup = microphoneGroup;
        clipSampleData = new float[sampleDataLength];
        scoreManager = GetComponent<ScoreManager>();

    }

    // Update is called once per frame
    void Update()
    {
        if(isRecording && isMuted == false){ //process the recording clip and update our score 
            ProcessClip();
        }
    }

    void ProcessClip(){
        
        currentUpdateTime += Time.deltaTime;
         if (currentUpdateTime >= updateStep) {

             currentUpdateTime = 0f;
             float[] data = new float[735];
            recordingAudioSource.GetOutputData(data, 0);
            //take the median of the recorded samples
            ArrayList s = new ArrayList();
            foreach (float f in data){
                s.Add(Mathf.Abs(f));
            }
            s.Sort();
            float clipLoudness = (float)s[735 / 2];

            if(clipLoudness >= clipLoudnessThreshold){
                 OnTalking();
             } else{
                 OnNoTalking();
             }
         }
    }


    void OnTalking(){ //event triggers when you are talking
        scoreManager.OnTalking();
    }

    void OnNoTalking(){ //event triggers when you are not talking
        scoreManager.OnNoTalking();
    }

    void OnFinishRecording(){ //calls when we are done recording
        isRecording = false;
            recordingAudioSource.Stop();
            playbackAudioSource.clip = recordingAudioSource.clip;
            Microphone.End(Microphone.devices[0]);
            playbackAudioSource.Play();
    }

    public void StartRecording(){
        isRecording = true;
        scoreManager.score = 0;
            recordingAudioSource.clip = Microphone.Start(Microphone.devices[0], true, 500, AudioSettings.outputSampleRate);
            while (!(Microphone.GetPosition(Microphone.devices[0]) > 0))
            {} 
            recordingAudioSource.Play();
    }

    public void ToggleMute(){
        isMuted = !isMuted;
        if(isMuted == false){
            scoreManager.testSprite.GetComponent<Image>().color = Color.gray;
        } else{
            scoreManager.testSprite.GetComponent<Image>().color = Color.white;
        }
        
    }
}
