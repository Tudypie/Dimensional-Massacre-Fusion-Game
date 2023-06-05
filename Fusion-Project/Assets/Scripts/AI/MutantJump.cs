using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutantJump : ChaserMonster
{

    [Header("Jump Attack")]
    public bool isDoingJumpAttack = false;
    public bool canDoJumpAttack = true;
    public bool jumpAttackDamagedPlayer = false;
    public bool playerIsInTrigger = false;
    public float jumpAttackRange = 10f;
    public float jumpAttackDamage = 150f;
    public float jumpDuration = 2f;
    public float inAirSpeed = 10f;
    public float inAirAcceleration = 200f;
    public float inAirHitSpeedReduction = 10f;
    public float airSpeedMultiplierOverTime = 1.5f;
    public float jumpAttackCooldown = 10f;

    private Vector3 jumpPosition;

    protected override void Update()
    {
        base.Update();

        if(isDoingJumpAttack)
            JumpAttack();
    }

    protected override void Chase()
    {
        if(canDoJumpAttack && Vector3.Distance(transform.position, playerTransform.position) <= jumpAttackRange
        && Vector3.Distance(transform.position, playerTransform.position) > attackRange)
        {   
            state = State.JumpAttack;
            canDoJumpAttack = false;
            isDoingJumpAttack = true;
            agent.speed = inAirSpeed;
            agent.acceleration = inAirAcceleration;
            Invoke("Landing", jumpDuration);
            jumpPosition = playerTransform.position;
            anim.Play("JumpAttack");
            JumpAttack();

            Debug.Log(gameObject.name + " is doing a jump attack at " + jumpPosition);
        }

        base.Chase();
    }

    protected override void TakeDamage()
    {
        if(isDoingJumpAttack)
        {
            agent.speed -= inAirHitSpeedReduction;
            agent.acceleration -= inAirHitSpeedReduction;
        }
        base.TakeDamage();
    }

    private void JumpAttack()
    {       
        agent.SetDestination(playerTransform.position);
    }
    private void Landing()
    {
        Debug.Log(gameObject.name + " has landed and cannot jump attack anymore for " + jumpAttackCooldown + " seconds");
        agent.SetDestination(transform.position);
        CameraShake.Instance.Shake(1.5f);
        isDoingJumpAttack = false;
        Invoke("ResetJumpAttack", jumpAttackCooldown);
        Invoke("GoBackToChase", 1f);

        if(playerIsInTrigger)
        {
            playerTransform.gameObject.GetComponent<Health>().TakeDamage(jumpAttackDamage);
            jumpAttackDamagedPlayer = true;
            Debug.Log(gameObject.name + " has damaged player on landing");
        }
    }

    private void GoBackToChase()
    {   
        agent.acceleration = initialAcceleration;
        agent.speed = initialSpeed;
        state = State.Chase;
    }

    private void ResetJumpAttack()
    {
        Debug.Log(gameObject.name + " can jump attack again");
        canDoJumpAttack = true;
        jumpAttackDamagedPlayer = false;
    }

    public void AttackPlayerInJumpAttack()
    {   
        if(state != State.JumpAttack)
            return;
        
        playerIsInTrigger = true;
        if(isDoingJumpAttack && canAttack)
        {
            canAttack = false;
            Invoke("AttackDelay", attackDelay);
            playerTransform.gameObject.GetComponent<Health>().TakeDamage(attackDamage);
            Debug.Log(gameObject.name + " has damaged player while doing jump attack");
        }

    }
    public void PlayerLeftTrigger()
    {
        playerIsInTrigger = false;
    }

}
