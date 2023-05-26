using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChasePlayer : MonoBehaviour
{   
    private Transform playerTransform;
    private NavMeshAgent agent;
    private Animator anim;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        agent.SetDestination(playerTransform.position);
        anim.SetBool("Chase", true);
    }


}
