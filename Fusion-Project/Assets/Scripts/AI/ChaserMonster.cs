using UnityEngine;
using UnityEngine.AI;

public class ChaserMonster : MonoBehaviour
{   
    public enum State
    {
        Idle,
        Chase,
        Attack,
        Stunned,
        Dead,
        JumpAttack
    }

    public State state;

    [Header("Audio")]
    [SerializeField] private AudioClip damageSound;
    [SerializeField] private AudioClip deathSound;

    [Header("Monster Settings")]
    public float initialSpeed;
    public float initialAcceleration;
    public float chaseDistance = 20f;
    public float attackDelay = 1f;
    public float attackDamage = 15f;
    public float attackRange = 3f;
    public float hitStunDuration = 0.5f;
    public int dropPickupChance = 5;
    public float colliderHeightIncrease = 2f;
    public bool canAttack = true;

    [HideInInspector] public CapsuleCollider monsterCollider;
    [HideInInspector] public float initialColliderHeight;
    [HideInInspector] public Transform playerTransform;
    [HideInInspector] public Camera mainCamera;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Animator anim;
    [HideInInspector] public AudioSource audioSource;

    protected virtual void Start()
    {   
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        monsterCollider = GetComponent<CapsuleCollider>();
        initialColliderHeight = monsterCollider.height;
        mainCamera = Camera.main;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        initialSpeed = agent.speed;
        initialAcceleration = agent.acceleration;
    }

    protected virtual void Update()
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

        if(state != State.Idle)
            return;

        if (Vector3.Distance(transform.position, playerTransform.position) > chaseDistance)
            state = State.Idle;
        else
            state = State.Chase;
        
    }

    protected virtual void Chase()
    {
        agent.SetDestination(playerTransform.position);
        anim.SetBool("Chase", true);

        MusicController.Instance.playerIsChased = true;

        if(Vector3.Distance(transform.position, playerTransform.position) <= attackRange)
        {
            //Debug.Log("Player got in attack range of " + gameObject.name);
            state = State.Attack;
        }
        else if (Vector3.Distance(transform.position, playerTransform.position) > chaseDistance*2f)
            state = State.Idle;
    }

    private void Attack()
    {
        anim.SetBool("Attack", true);

        if(Vector3.Distance(transform.position, playerTransform.position) > attackRange)
        {
            //Debug.Log("Player got out of attack range of" + gameObject.name);
            anim.SetBool("Attack", false);
            state = State.Chase;
        }

        if(!canAttack)
            return;

        canAttack = false;

        //Debug.Log(gameObject.name + " damaged player for " + attackDamage + " damage");

        playerTransform.gameObject.GetComponent<Health>().TakeDamage(attackDamage);

        Invoke("AttackDelay", attackDelay);
    }

    private void AttackDelay() => canAttack = true;

    private void Idle()
    {
        agent.SetDestination(transform.position);
        anim.SetBool("Chase", false);
    }

    protected virtual void TakeDamage()
    {
        CameraShake.Instance.StartCoroutine(CameraShake.Instance.Shake(1.1f, 0.4f));
        audioSource.PlayOneShot(damageSound);

        if(state == State.JumpAttack)
            return;

        agent.SetDestination(transform.position);
        anim.SetBool("TakeDamage", true);
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
