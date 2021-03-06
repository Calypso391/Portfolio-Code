﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject target; // future use?
    public float speed = 0.8f;
    private Vector3 targetPosition;
    private float timer = 60.0f;
    
    void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        targetPosition = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        float step =  speed * Time.deltaTime; // calculate distance to move
        timer -= timer * Time.deltaTime;
        // Leave some space between
        if (Vector3.Distance(transform.position, targetPosition) > 2.0f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
        }
        if (Vector3.Distance(transform.position, targetPosition) > 30.0f || timer <= 0)
        {
            SpawnEnemies.enemyCount--;
            Destroy(gameObject);
        }
    }
}
