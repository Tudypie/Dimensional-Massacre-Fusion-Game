using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnEnterTriggerEvent : MonoBehaviour
{
    [SerializeField] private UnityEvent onEnterEvent;
    [SerializeField] private UnityEvent onExitEvent;

    [SerializeField] private bool oneTimeTrigger = true;

    private void OnTriggerEnter(Collider other)
    {   
        if(other.gameObject.tag != "Player")
            return;
        
        onEnterEvent?.Invoke();

        if(oneTimeTrigger)
            GetComponent<Collider>().enabled = false;
    }

    private void OnTriggerExit(Collider other)
    {   
        if(other.gameObject.tag != "Player")
            return;
        
        onExitEvent?.Invoke();
    }
}
