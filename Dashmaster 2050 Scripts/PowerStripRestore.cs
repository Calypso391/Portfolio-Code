using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerStripRestore : RestoringObject
{
    
    public Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        //Key.SetActive(false);
        this.GetComponent<ParticleSystem>().Stop();
    }


    public override void TriggerObject(){ //TODO: sfx
        triggered = true;
        animator.SetTrigger("trigger");
        this.GetComponent<ParticleSystem>().Play();
       
    }

    public override void UnTriggerObject()
     {
          animator.Play("default");
            this.GetComponent<ParticleSystem>().Stop();
           RestoreScreen(-1);
            triggered = false;
            
     }

    /*private void OnMouseDown() {
        if(triggered){
            animator.Play("default");
            this.GetComponent<ParticleSystem>().Stop();
            RestoreScreen();
            triggered = false;
            Key.SetActive(false);
        }
    }*/
}
