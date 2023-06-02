using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

public class RotateableObject : MonoBehaviour
{
    [SerializeField] private NavMeshSurface surface;

    [SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private float reverseDirections = 1;
     private Vector3 startRotation;
    [SerializeField] private Vector3 endRotation;
    [SerializeField] private bool startRotationMovement = false;
    [SerializeField] private bool isRotatingForward = false; 
    [SerializeField] private bool finishedRotation;

    private void Start()
    {   
        if(GetComponent<NavMeshSurface>() != null)
            surface = GetComponent<NavMeshSurface>();
        startRotation = transform.rotation.eulerAngles;
    }

    private void Update()
    {
        if(!startRotationMovement)
            return;

        if(finishedRotation && isRotatingForward)
            return;

        if(Vector3.Distance(transform.rotation.eulerAngles, endRotation) < 0.2f && isRotatingForward)
        {   
            finishedRotation = true;
            NavMeshBaker.Instance.BakeNavigation(surface);
            return;
        }
        else if(Vector3.Distance(transform.rotation.eulerAngles, startRotation) < 0.2f && !isRotatingForward)
        {
            finishedRotation = false;
            startRotationMovement = false;
            return;
        }

        if (isRotatingForward)
        {
            transform.Rotate(Vector3.up * reverseDirections * Time.deltaTime * rotationSpeed);
        }
        else
        {
            transform.Rotate(Vector3.down * reverseDirections * Time.deltaTime * rotationSpeed);
        }
    }

    public void StartRotation()
    {
        startRotationMovement = true;
        isRotatingForward = true;
    }

    public void RotateBack()
    {
        isRotatingForward = false;
    }
}
