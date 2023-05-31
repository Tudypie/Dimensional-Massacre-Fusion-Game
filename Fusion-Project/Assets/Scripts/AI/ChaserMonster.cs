using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class ChaserMonster : MonoBehaviour
{   

    enum State
    {
        Idle,
        Chase,
        Attack,
        Dead
    }

    [SerializeField] private State state;

    [SerializeField] public float chaseDistance = 20f;
    [SerializeField] private float attackDelay = 1f;
    [SerializeField] private float attackDamage = 15f;
    [SerializeField] private float attackRange = 3f;
    [SerializeField] public int dropPickupChance = 5;
    private bool canAttack = true;

    [Header("References")]
    [SerializeField] private Collider monsterCollider;
    [SerializeField] private AudioClip damageSound;
    [SerializeField] private AudioClip deathSound;
    private Transform playerTransform;
    private Camera mainCamera;
    private NavMeshAgent agent;
    private Animator anim;
    private AudioSource audioSource;
    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        mainCamera = Camera.main;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {   
        if(state == State.Dead)
            return;

        switch(state)
        {
            case State.Chase:
                Chase();
                break;
            case State.Attack:
                Attack();
                break;
            case State.Idle:
                Idle();
                break;
        }

        if(state == State.Attack)
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

        if(Vector3.Distance(transform.position, playerTransform.position) <= attackRange)
        {
            Debug.Log("Player got in attack range of " + gameObject.name);
            state = State.Attack;
        }
    }

    private void Attack()
    {
        if(Vector3.Distance(transform.position, playerTransform.position) > attackRange)
        {
            Debug.Log("Player got out of attack range of" + gameObject.name);
            state = State.Chase;
        }

        if(!canAttack)
            return;

        canAttack = false;

        Debug.Log(gameObject.name + " damaged player for " + attackDamage + " damage");

        playerTransform.gameObject.GetComponent<Health>().TakeDamage(attackDamage);

        Invoke("AttackDelay", attackDelay);
    }

    private void AttackDelay() => canAttack = true;

    private void Idle()
    {
        agent.SetDestination(transform.position);
        anim.SetBool("Chase", false);
    }

    public void TakeDamage()
    {
        mainCamera.GetComponent<CameraShake>().Shake(0.8f);

        audioSource.PlayOneShot(damageSound);
    }

    public void Die()
    {
        state = State.Dead;

        agent.SetDestination(transform.position);
        anim.SetBool("Chase", false);
        anim.Play("Death");

        audioSource.Stop();
        audioSource.PlayOneShot(deathSound);

        monsterCollider.enabled = false;

        Destroy(gameObject, 1.5f);

        int random = Random.Range(0, dropPickupChance);
        if(random == 0)
            SpawnPickupable.Instance.Spawn(transform);

        PlayerStats.Instance.AddKill();
    }

}
