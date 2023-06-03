using UnityEngine;
using UnityEngine.AI;

public class ChaserMonster : MonoBehaviour
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

    [Header("Monster Settings")]

    [SerializeField] public float chaseDistance = 20f;
    [SerializeField] private float attackDelay = 1f;
    [SerializeField] private float attackDamage = 15f;
    [SerializeField] private float attackRange = 3f;
    [SerializeField] private float hitStunDuration = 0.5f;
    [SerializeField] public int dropPickupChance = 5;
    [SerializeField] private float colliderHeightIncrease = 2f;
    private bool canAttack = true;

    [Header("Audio")]
    [SerializeField] private AudioClip damageSound;
    [SerializeField] private AudioClip deathSound;

    private CapsuleCollider monsterCollider;
    private float initialColliderHeight;
    private Transform playerTransform;
    private Camera mainCamera;
    private NavMeshAgent agent;
    private Animator anim;
    private AudioSource audioSource;
    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        monsterCollider = GetComponent<CapsuleCollider>();
        initialColliderHeight = monsterCollider.height;
        mainCamera = Camera.main;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {   
        if(state == State.Dead || state == State.Stunned || agent.enabled == false)
            return;

        if(Camera.main.orthographic)
            monsterCollider.height = colliderHeightIncrease;
        else
            monsterCollider.height = initialColliderHeight;   

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

        if(state == State.Attack || state == State.Chase)
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

        MusicController.Instance.playerIsChased = true;

        if(Vector3.Distance(transform.position, playerTransform.position) <= attackRange)
        {
            Debug.Log("Player got in attack range of " + gameObject.name);
            state = State.Attack;
        }
        else if (Vector3.Distance(transform.position, playerTransform.position) > chaseDistance*1.6f)
            state = State.Idle;
    }

    private void Attack()
    {
        anim.SetBool("Attack", true);

        if(Vector3.Distance(transform.position, playerTransform.position) > attackRange)
        {
            Debug.Log("Player got out of attack range of" + gameObject.name);
            anim.SetBool("Attack", false);
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
        agent.SetDestination(transform.position);
        anim.SetBool("TakeDamage", true);
        mainCamera.GetComponent<CameraShake>().Shake(0.8f);
        audioSource.PlayOneShot(damageSound);
        state = State.Stunned;
        Invoke("EndDamageStun", hitStunDuration);
    }

    private void EndDamageStun()
    {   
        if(state == State.Dead)
            return;

        anim.SetBool("TakeDamage", false);
        state = State.Chase;
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

        MusicController.Instance.playerIsChased = false;

        Destroy(gameObject, 1.5f);

        int random = Random.Range(0, dropPickupChance);
        if(random == 0)
            SpawnPickupable.Instance.Spawn(transform);

        PlayerStats.Instance.AddKill();
    }

}
