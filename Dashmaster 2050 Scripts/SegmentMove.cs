using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentMove : MonoBehaviour
{
    float moveSpeed = 4.0f;

    private void Start()
    {
        moveSpeed = InfiniteRunnerGenerator.Instance.currentSpeed;
    }

    private void Update()
    {
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;
    }
}
