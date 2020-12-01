using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BackgroundAppear : MonoBehaviour
{
    public float localPosAppear;
    public float localPosDisappear;
    public float animationTime = 0.5f;
    public void Appear()
    {
        transform.DOLocalMoveY(localPosAppear, animationTime).SetEase(Ease.OutBounce);
    }

    public void Disappear()
    {
        StartCoroutine(DoDisapear());
    }

    IEnumerator DoDisapear()
    {
        transform.DOLocalMoveY(localPosDisappear, animationTime).SetEase(Ease.InBack);
        yield return new WaitForSeconds(animationTime + .1f);
        gameObject.SetActive(false);
    }


}
