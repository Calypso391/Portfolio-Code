using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempEnemy : MonoBehaviour
{
    public GameObject player;
    private Rigidbody rb;
    public float speed = 0.5f;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float step = speed * Time.fixedDeltaTime;
        Vector3 direction = player.transform.position - gameObject.transform.position;
        rb.MovePosition(gameObject.transform.position + direction * step);
        //transform.position = Vector3.MoveTowards(gameObject.transform.position, player.transform.position, step);
    }
}
