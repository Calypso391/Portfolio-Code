using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour
{
    public float Speed = 1.0f;
    public Vector2 Direction = Vector2.left;
    public EColorFlag color = EColorFlag.None;
    public List<AudioClip> SuccessSounds = new List<AudioClip>();
    public List<AudioClip> FailSounds = new List<AudioClip>();
    public IntVariable Score = null;
    public bool DebugCycleThroughColors = false;
    private float timer = 0.0f;

    private Rigidbody2D rb = null;

    /// <summary>
    /// If the player successfully matches the wall
    /// </summary>
    public void Clear()
    {
        AudioSource.PlayClipAtPoint(SuccessSounds[Random.Range(0, SuccessSounds.Count - 1)], new Vector3(0.0f, 0.0f, -10.0f));
        Score.ApplyChange(1);
        GetComponent<BoxCollider2D>().enabled = false;
        StartCoroutine(Merge());
        //Destroy(gameObject);
    }

    /// <summary>
    /// If the player fails to match the wall
    /// </summary>
    public void Fail()
    {
        AudioSource.PlayClipAtPoint(FailSounds[Random.Range(0, FailSounds.Count - 1)], new Vector3(0.0f, 0.0f, -10.0f));
        GetComponent<BoxCollider2D>().enabled = false;
        StartCoroutine(Explode());
        //Destroy(gameObject);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        Color rgb = EColorRGBHelper.FromSingleFlagToRGB(color);

        var spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        foreach (var sprite in spriteRenderers) {
            sprite.color = rgb;
        }
    }

    private void Update()
    {
        //var newPosition = (Vector2)transform.position + Direction * Speed * Time.deltaTime;
        //rb.MovePosition(newPosition);
        transform.position += Time.deltaTime * Vector3.left * Speed;


        // helps me quickly visualize every color is working by cycling through them
        // DEBUG ONLY
        if (DebugCycleThroughColors) {
            timer += Time.deltaTime;
            if (timer > 1.0f) {
                timer = 0.0f;
                int nextColor = ((int)color) << 1;
                if (nextColor > 0x800) { // wrap
                    nextColor = 0x1;
                }
                color = (EColorFlag)nextColor;

                Color rgb = EColorRGBHelper.FromSingleFlagToRGB(color);

                var spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
                foreach (var sprite in spriteRenderers) {
                    sprite.color = rgb;
                }
            }
        }
    }


    public IEnumerator Merge()
    {
        float effectTimer = 0.0f;

        while (effectTimer < .3f)
        {
            effectTimer += Time.deltaTime;
            Vector3 tempScale = transform.localScale;
            float newScale = Mathf.Sin(Time.time * 20) * .5f;
            tempScale.x += newScale;
            tempScale.y += newScale;
            transform.localScale = tempScale;
            yield return null;
        }

        Destroy(gameObject);
    }

    public IEnumerator Explode()
    {
        float effectTimer = 0.0f;

        while (effectTimer < .3f)
        {
            effectTimer += Time.deltaTime;
            Vector3 tempScale = transform.localScale;
            float newScale = Mathf.Sin(Time.time * 20) * 1.5f;
            tempScale.x = newScale;
            tempScale.y = newScale;
            transform.localScale = tempScale;
            yield return null;
        }

        Destroy(gameObject);
    }
}
