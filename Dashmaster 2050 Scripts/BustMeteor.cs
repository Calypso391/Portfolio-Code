using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class BustMeteor : MonoBehaviour
{
    public Vector3 MeteorHitSpot;
    public float animationTime;
    public SegmentMove segmentMove;
    public float screenShakeDur = 0.2f;
    public float screenShakeIntensity = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DoMeteorRoutine());
    }

    IEnumerator DoMeteorRoutine()
    {
        transform.DOMove(MeteorHitSpot, animationTime).SetEase(Ease.OutCubic);
        GetComponent<TrailRenderer>().enabled = false;
        yield return new WaitForSeconds(animationTime);
        GetComponent<AudioSource>().Play();
        ScreenShake.instance.TriggerShake(screenShakeDur, screenShakeIntensity);
        segmentMove.enabled = true;
    }
}
