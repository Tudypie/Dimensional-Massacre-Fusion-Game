using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StayOnGround : MonoBehaviour
{
    [SerializeField] float yOffset = 1f;
    [SerializeField] float startTimer = 1.3f;
    private float timer = 0f;
    private void Update()
    {   
        if(timer > 0f)
        {
            timer -= Time.deltaTime;

            RaycastHit hit;
            if (Physics.Raycast(transform.position + new Vector3(0f, 100f, 0f), Vector3.down, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
            {
                transform.position = new Vector3(transform.position.x, hit.point.y + yOffset, transform.position.z);
            }


            return;
        }

        if(Input.GetKeyDown(KeyCode.Tab)) 
        {
            timer = startTimer;

            if(GetComponent<NavMeshAgent>() != null)
            {
                GetComponent<NavMeshAgent>().enabled = false;
                Invoke("ActivateNavMeshAgent", startTimer);
            }
        }

    }

    private void ActivateNavMeshAgent()
    {
        if(GetComponent<NavMeshAgent>() != null)
            GetComponent<NavMeshAgent>().enabled = true;
    }
}
