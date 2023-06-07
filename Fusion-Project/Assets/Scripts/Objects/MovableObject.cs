using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

public class MovableObject : MonoBehaviour
{
    [SerializeField] private NavMeshSurface surface;

    [SerializeField] private float movingSpeed = 1f;
    [SerializeField] private float reverseDirections = 1;
     private Vector3 startPosition;
    [SerializeField] private Vector3 endPosition;
    [SerializeField] private bool startMovement = false;
    [SerializeField] private bool isMovingForward = false;
    [SerializeField] private bool hasReachedEndPosition = false;


    private void Start()
    {
        if(GetComponent<NavMeshSurface>() != null)
            surface = GetComponent<NavMeshSurface>();
        startPosition = transform.position;
    }

    private void Update()
    {   
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, startPosition.x, endPosition.x), transform.position.y, 
        Mathf.Clamp(transform.position.z, startPosition.z, endPosition.z));
        
        if(!startMovement)
            return;

        if(hasReachedEndPosition && isMovingForward)
            return;

        if(Vector3.Distance(new Vector3(transform.position.x, 0f, transform.position.z), 
        new Vector3(endPosition.x, 0f, endPosition.z)) < 0.2f && isMovingForward)
        {   
            hasReachedEndPosition = true;
            NavMeshBaker.Instance.BakeNavigation();
            return;
        }
        else if(Vector3.Distance(new Vector3(transform.position.x, 0f, transform.position.z), 
        new Vector3(startPosition.x, 0f, startPosition.z)) < 0.2f && !isMovingForward)
        {
            hasReachedEndPosition = false;
            startMovement = false;
            return;
        }

        if(isMovingForward)
        {
            transform.Translate(Vector3.forward * reverseDirections * Time.deltaTime * movingSpeed);
        }
        else
        {
            if(Vector3.Distance(transform.position, startPosition) < 0.2f)
            {
                hasReachedEndPosition = false;
                startMovement = false;
                return;
            }

            transform.Translate(Vector3.back * reverseDirections * Time.deltaTime * movingSpeed);
        }
    }

    public void StartMovement()
    {
        startMovement = true;
        isMovingForward = true;
    }

    public void MoveBack()
    {
        isMovingForward = false;
    }
}
