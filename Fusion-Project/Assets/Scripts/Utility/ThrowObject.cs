using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ThrowObject : MonoBehaviour
{
    public float throwForce;
    [SerializeField] float amount;
    [SerializeField] Transform spawnPoint;
    [SerializeField] GameObject objectToThrow;
    [SerializeField] UnityEvent OnThrow;

    public void Throw()
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject throwable = Instantiate(objectToThrow, spawnPoint.position, transform.rotation);
            throwable.GetComponent<Throwable>().throwForce = throwForce;
            OnThrow?.Invoke();
        }


    }
}
