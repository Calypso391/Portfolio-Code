/* Created by user DerHugo
 * 
 * Reference: https://stackoverflow.com/a/54303138
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedInvoker : MonoBehaviour
{
    [SerializeField] private bool InvokeOnStart = false;
    [SerializeField] private List<EventDelayPair> EventDelayPairs = null;

    public void Invoke()
    {
        foreach(var item in EventDelayPairs) {
            StartCoroutine(InvokeDelayed(item.unityEvent, item.Delay));
        }
    }

    private void Start()
    {
        if (InvokeOnStart) {
            Invoke();
        }
    }

    private IEnumerator InvokeDelayed(UnityEngine.Events.UnityEvent unityEvent, float delay)
    {
        yield return new WaitForSeconds(delay);

        unityEvent.Invoke();
    }

    [System.Serializable]
    private class EventDelayPair
    {
        public UnityEngine.Events.UnityEvent unityEvent = null;
        public float Delay = 0.0f;
    }
}
