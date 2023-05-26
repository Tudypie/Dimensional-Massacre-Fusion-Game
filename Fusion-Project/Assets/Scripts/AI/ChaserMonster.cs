using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaserMonster : MonoBehaviour
{   

    enum State
    {
        Idle,
        Chase,
        Attack
    }

    [SerializeField] private State state;

    [SerializeField] private float chaseDistance = 20f;
    [SerializeField] private float attackDelay = 1f;
    private bool canAttack = true;


    //References
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
        switch(state)
        {
            case State.Chase:
                Chase();
                break;
            case State.Attack:
                Attack();
                break;
        }

        if(state == State.Chase || state == State.Attack)
            return;

        if (Vector3.Distance(transform.position, playerTransform.position) > chaseDistance)
            state = State.Idle;
        else
            state = State.Chase;
        
    }

    private void Chase()
    {
        agent.SetDestination(playerTransform.position);
        anim.SetBool("Chase", true);
    }

    private void Attack()
    {
        if(!canAttack)
            return;

        StartCoroutine(AttackSequence());
    }

    private IEnumerator AttackSequence()
    {   
        canAttack = false;
        playerTransform.gameObject.GetComponent<Health>().TakeDamage(15f);
        yield return new WaitForSeconds(attackDelay);
        canAttack = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {   
            state = State.Attack;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            state = State.Chase;
        }
    }






}
