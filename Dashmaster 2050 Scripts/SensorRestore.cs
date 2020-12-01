using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorRestore : RestoringObject
{
    Vector3 startPos;
    Quaternion startRot;
    public GameObject mainCam;
    Camera cam;
  
  
    // Start is called before the first frame update
    void Start()
    {
        startPos = this.transform.position;
        startRot = this.transform.rotation;   
        cam = mainCam.GetComponent<Camera>();
        this.GetComponent<ParticleSystem>().Pause();
    }

   

    public override void TriggerObject(){ //todo: sfx

        
        this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        this.GetComponent<Rigidbody2D>().AddTorque(35f);
        triggered = true;
        this.GetComponent<ParticleSystem>().Play();
       
    }

    public override void UnTriggerObject()
     {
         if(triggered){
             triggered = false;
            this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            this.GetComponent<Rigidbody2D>().angularVelocity = 0;
            this.transform.position = startPos;
            this.transform.rotation = startRot;
            this.GetComponent<ParticleSystem>().Stop();
            
            RestoreScreen(-1);
         }
          
     }

    /*private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject == triggerArea && triggered){
            isDragging = false;
            triggered = false;
            this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            this.transform.position = startPos;
            StartCoroutine(changeRotation());
            this.GetComponent<ParticleSystem>().Stop();
            triggerArea.GetComponent<ParticleSystem>().Stop();
            Key.SetActive(false);
            RestoreScreen();
        }
    }*/

    IEnumerator changeRotation(){
        while(true){
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, startRot, 8.5f * Time.deltaTime);
            if(this.transform.rotation == startRot){
                break;
            }
            yield return null;
        }
    }

    IEnumerator changePos(){
        while(true){
            this.transform.position = Vector3.Lerp(this.transform.position, startPos, 8.5f * Time.deltaTime);
            if(this.transform.position == startPos){
                break;
            }
            yield return null;
        }
    }

    
}
