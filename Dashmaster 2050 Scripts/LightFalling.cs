using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFalling : RestoringObject
{
    Vector3 startPos;

    public GameObject mainCam;
    Camera cam;
  
    // Start is called before the first frame update
    void Start()
    {
        startPos = this.transform.position;   
        cam = mainCam.GetComponent<Camera>();
        this.GetComponent<ParticleSystem>().Pause();
        
    }


    public override void TriggerObject(){ //todo: sfx
        this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        this.GetComponent<Rigidbody2D>().AddTorque(10f);
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
            this.transform.rotation = Quaternion.identity; 
            this.GetComponent<ParticleSystem>().Stop();
            
            RestoreScreen(-1);
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

     /*private void OnTriggerEnter2D(Collider2D other) {
         if(other.gameObject == triggerArea && triggered){
             isDragging = false;
             triggered = false;
             this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
             this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
             this.transform.position = startPos;
             this.transform.rotation = Quaternion.identity;
             this.GetComponent<ParticleSystem>().Stop();
             triggerArea.GetComponent<ParticleSystem>().Stop();
             Key.SetActive(false);
             RestoreScreen();
         }
     }*/



    /*private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject == triggerArea && triggered){
            isDragging = false;
            triggered = false;
            this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            this.transform.position = startPos;
            this.transform.rotation = Quaternion.identity;
            this.GetComponent<ParticleSystem>().Stop();
            triggerArea.GetComponent<ParticleSystem>().Stop();
            Key.SetActive(false);
            RestoreScreen();
        }
    }*/

    
}
