using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentMove : MonoBehaviour
{
    public float moveSpeed = 4.0f;

    private void Update()
    {
        transform.position += Vector3.left * moveSpeed * InfiniteRunnerGenerator.Instance.currentSpeedMult * Time.deltaTime;
    }
}
