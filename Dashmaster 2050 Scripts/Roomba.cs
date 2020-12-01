using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roomba : MonoBehaviour
{
    public Vector2 xRange;
    public GameObject blinkinglight;

    private void Start()
    {
        StartCoroutine(DoBlinkingLight());
    }

    IEnumerator DoBlinkingLight()
    {
        while(true)
        {
            blinkinglight.SetActive(false);
            yield return new WaitForSeconds(0.1f);
            blinkinglight.SetActive(true);
            yield return new WaitForSeconds(0.3f);
        }
    }

    private void Update()
    {
        if(transform.position.x > xRange.y)
        {
            transform.right = transform.right * -1;
            transform.position += transform.right * Time.deltaTime * 2;
        }
        else if(transform.position.x < xRange.x)
        {
            transform.right = transform.right * -1;
            transform.position += transform.right * Time.deltaTime * 2;
        }
        else
        {
            transform.position += transform.right * Time.deltaTime * 2;
        }
    }
}
