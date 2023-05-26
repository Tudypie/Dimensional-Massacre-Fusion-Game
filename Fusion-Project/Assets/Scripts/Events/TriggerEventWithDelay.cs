using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEventWithDelay : MonoBehaviour
{
    [SerializeField] private UnityEvent Event;

    public void TriggerEvent(float delay)
    {
        StartCoroutine(TriggerEventCoroutine(delay));
    }

    private IEnumerator TriggerEventCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        Event?.Invoke();
    }
}
