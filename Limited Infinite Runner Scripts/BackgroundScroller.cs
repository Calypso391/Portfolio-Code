using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    public float moveSpeed;
    public float xToResetAt = 25.85f;
    public float xToResetTo = 57.72f;

    private void Update()
    {
        transform.position += Time.deltaTime * Vector3.left * moveSpeed * InfiniteRunnerGenerator.Instance.currentSpeedMult;
        if(transform.position.x < xToResetAt)
        {
            transform.position = new Vector3(xToResetTo, transform.position.y, 0);
        }
    }
}
