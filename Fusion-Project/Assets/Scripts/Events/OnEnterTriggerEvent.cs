using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnEnterTriggerEvent : MonoBehaviour
{
    [SerializeField] private UnityEvent onEnterEvent;
    [SerializeField] private UnityEvent onExitEvent;

    [SerializeField] private bool oneTimeTrigger = true;
    [SerializeField] private bool stayInTrigger = false;
    [SerializeField] private LayerMask layerMask = 3;

    private void OnTriggerEnter(Collider other)
    {   
        if (((1 << other.gameObject.layer) & layerMask.value) == 0)
            return;
        
        onEnterEvent?.Invoke();

        if(oneTimeTrigger && onExitEvent == null)
            GetComponent<Collider>().enabled = false;
    }

    private void OnTriggerStay(Collider other)
    {   
        if(!stayInTrigger)
            return;

        if (((1 << other.gameObject.layer) & layerMask.value) == 0)
            return;
        
        onEnterEvent?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {   
        if (((1 << other.gameObject.layer) & layerMask.value) == 0)
            return;
        
        onExitEvent?.Invoke();

        if(oneTimeTrigger)
            GetComponent<Collider>().enabled = false;
    }
}
