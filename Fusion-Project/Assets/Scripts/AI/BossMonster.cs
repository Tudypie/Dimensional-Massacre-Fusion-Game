using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class BossMonster : MonoBehaviour
{   

    enum State
    {
        Idle,
        Chase,
        Attack,
        Stunned,
        Dead
    }

    [SerializeField] private State state;

    [SerializeField] private float chaseDistance = 20f;
    [SerializeField] private float attackDelay = 1f;
    [SerializeField] private float attackDamage = 15f;
    private bool canAttack = true;

    [SerializeField] private int timesHit;
    [SerializeField] private int hitsTillStun = 25;
    [SerializeField] private int stunShieldHealth = 3000;

    [SerializeField] private UnityEvent OnAttack;

    [SerializeField] private UnityEvent OnStun;

    //References

    [SerializeField] private MeshCollider meshCollider;
    [SerializeField] private EnemySpawner enemySpawner;
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
            case State.Stunned:
                Stunned();
                break;
        }

        if(state == State.Chase || state == State.Attack || state == State.Stunned)
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

        if(Vector3.Distance(transform.position, playerTransform.position) <= agent.stoppingDistance)
        {
            state = State.Attack;
        }

        if(Vector3.Distance(transform.position, playerTransform.position) > chaseDistance)
        {
            state = State.Idle;
        }
    }

    private void Attack()
    {
        if(Vector3.Distance(transform.position, playerTransform.position) > agent.stoppingDistance)
        {
            state = State.Chase;
        }

        if(!canAttack)
            return;

        OnAttack?.Invoke();

        canAttack = false;
        Debug.Log(gameObject.name + " damaged player for " + attackDamage + " damage");
        playerTransform.gameObject.GetComponent<Health>().TakeDamage(attackDamage);
        Invoke("AttackDelay", attackDelay);
    }

    private void AttackDelay() => canAttack = true;

    private void Stunned()
    {      
        anim.SetBool("Chase", false);
        agent.SetDestination(transform.position);

        audioSource.mute = true;
        enemySpawner.enabled = true;

        GetComponent<BossHealthbar>().ChangeColorToBlue();

        if(GetComponent<Shield>().shieldHealth <= 0)
        {
            state = State.Chase;

            audioSource.mute = false;
            enemySpawner.enabled = false;

            GetComponent<BossHealthbar>().ChangeColorToRed();
        }

    }


    public void TakeDamage()
    {
        CameraShake.Instance.StartCoroutine(CameraShake.Instance.Shake(0.8f, 0.2f));

        audioSource.PlayOneShot(AudioPlayer.Instance.monsterHit);

        if(state == State.Stunned || state == State.Dead)
            return;

        timesHit++;

        if(timesHit >= hitsTillStun)
        {
            state = State.Stunned;
            GetComponent<Shield>().shieldHealth = stunShieldHealth;
            OnStun?.Invoke();
            timesHit = 0;       
            agent.speed += 1;
        }

    }

    public void Die()
    {
        state = State.Dead;

        agent.SetDestination(transform.position);
        anim.SetBool("Chase", false);

        audioSource.Stop();
        audioSource.PlayOneShot(AudioPlayer.Instance.monsterDeath);

        meshCollider.enabled = false;

        anim.Play("Death");

        PlayerStats.Instance.AddKill();
    }

    private void OnTriggerEnter(Collider other)
    {   
        if(state == State.Stunned || state == State.Dead)
            return;

        if (other.tag == "Player")
        {   
            state = State.Attack;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(state == State.Stunned || state == State.Dead)
            return;

        if (other.tag == "Player")
        {
            state = State.Chase;
        }
    }






}
