using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
public class SetPlayerName : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<TextMeshProUGUI>().text = "-" + System.Environment.UserName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
