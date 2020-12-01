using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    public bool isJumping = false;
    public bool isDucking = false;

    public float duckTime = 0.5f;
    public float jumpTime = 0.5f;
    public float jumpForce = 100f;
    public float initialJumpSpeed = 10f;
    public float maxHoldJumpTime;
    public float timer = 0.0f;
    public float jumpRate = .2f;

    private float originalYPos;
    public float jumpHeight = 0.5f;
    public float duckHeight = 0;

    Collider2D playerCollider;

    public CameraController cameraController;
    public AudioSource audioSource;
    public SoundEffectManager SEM;

    public float screenShakeDur = 0.25f;
    public float screenShakeIntensity = 0.25f;

    bool hasHitGround = false;
    float jumpKeyHeldTime = 0.0f;
    // Start is called before the first frame update

    private void Awake()
    {
    }
    void Start()
    {
        animator = GetComponent<Animator>();
        playerCollider = GetComponent<Collider2D>();
        originalYPos = transform.position.y;
    }

    // Update is called once per frame


    private void Update()
    {
        if (isJumping && hasHitGround)
        {
            isJumping = false;
            hasHitGround = false;
            animator.SetBool("Jumping", false);
            Vector2 pos = transform.position;
            pos.y = originalYPos;
            transform.position = pos;
        }

        if (timer <= 0)
        {
            if (isDucking)
            {
                isDucking = false;
                animator.SetBool("Ducking", false);
            }
        }
        else
        {
            timer -= Time.deltaTime;
        }

        if (!isJumping && !isDucking && !CameraController.dead)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                animator.SetBool("Jumping", true);
                animator.SetBool("Ducking", false);
                isJumping = true;
                timer = jumpTime;
                if (SEM != null)
                {
                    SEM.PlayClip(1);
                    SEM.PlaySubClip(0);
                }
                if (CameraController.controls.activeSelf) CameraController.controls.SetActive(false);
                GetComponent<Rigidbody2D>().velocity = (Vector2.up * initialJumpSpeed);
                jumpKeyHeldTime = 0;
            }

            else if (Input.GetKeyDown(KeyCode.S))
            {
                animator.SetBool("Jumping", false);
                animator.SetBool("Ducking", true);
                isDucking = true;
                timer = duckTime / InfiniteRunnerGenerator.Instance.currentSpeedMult;
                if (SEM != null)
                {
                    SEM.PlaySubClip(0);
                }
                if (CameraController.controls.activeSelf) CameraController.controls.SetActive(false);
            }
        }
    }
    void FixedUpdate()
    {
        if(isDucking)
        {
            //playerCollider.offset = new Vector2(0, duckHeight * .75f);
        }
        else
        {
            playerCollider.offset = Vector2.zero;   
        }

        if (isJumping)
        {
            if (GetComponent<AudioSource>() != null)
                GetComponent<AudioSource>().Pause();
            float percent = 1 - (timer / jumpTime);
            percent = Mathf.Clamp01(percent);
            percent *= -1;
        }
        else if(GetComponent<AudioSource>() != null)
            GetComponent<AudioSource>().UnPause();

        if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && jumpKeyHeldTime <= maxHoldJumpTime)
        {
            jumpKeyHeldTime += Time.deltaTime;
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpForce);
        }
        //if(animator.speed != 0)
        //animator.speed = InfiniteRunnerGenerator.Instance.currentSpeedMult;
        //if (audioSource != null) audioSource.pitch = InfiniteRunnerGenerator.Instance.currentSpeedMult;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(cameraController != null)
        {
            if (collision.gameObject.tag.Equals("Obstacle"))
            {
                cameraController.ChangeRange(1);
                ScreenShake.instance.TriggerShake(screenShakeDur,screenShakeIntensity);
                InfiniteRunnerGenerator.Instance.PlayerHit();
                StartCoroutine(DoPlayerHitEffects());
                StartCoroutine(DoBlockHitEffects(collision));
                if (!CameraController.dead)
                {
                    SEM.PlayClip(0);
                }
            }
            else if(collision.gameObject.tag.Equals("Duck Obstacle") && !isDucking)
            {
                cameraController.ChangeRange(1);
                ScreenShake.instance.TriggerShake(screenShakeDur, screenShakeIntensity);
                InfiniteRunnerGenerator.Instance.PlayerHit();
                StartCoroutine(DoPlayerHitEffects());
                StartCoroutine(DoBlockHitEffects(collision));
                if (!CameraController.dead)
                {
                    SEM.PlayClip(0);
                }
            }
            else if (collision.gameObject.tag.Equals("Recovery"))
            {
                cameraController.ChangeRange(-1);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "FloorReset")
        {
            hasHitGround = true;
        }
    }
    IEnumerator DoBlockHitEffects(Collider2D block)
    {
        block.GetComponent<Rigidbody2D>().isKinematic = false;
        block.GetComponent<Rigidbody2D>().AddForce(new Vector2(50, 10));
        block.GetComponent<Rigidbody2D>().AddTorque(250);
        block.gameObject.transform.parent = null;
        Destroy(block.gameObject, 5f);
        block.enabled = false;
        yield return null;
    }

    IEnumerator DoPlayerHitEffects()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
        animator.speed = 0;
        yield return new WaitForSeconds(0.25f);
        GetComponent<SpriteRenderer>().color = Color.white;
        animator.speed = InfiniteRunnerGenerator.Instance.currentSpeedMult;
    }
    float easeOutCubic(float x)
    {
        return Mathf.Sin((x * Mathf.PI) / 2);
    }
}
