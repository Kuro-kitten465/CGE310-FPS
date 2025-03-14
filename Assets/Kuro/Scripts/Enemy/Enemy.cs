using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IEnemy
{
    [Header("Enemy Settings")]
    [SerializeField] private float health = 100f;
    [SerializeField] private float damage = 15f;
    [SerializeField] private float attackCD = 1.5f;

    [Header("AI Settings")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private LayerMask whatIsGround, whatIsPlayer;
    private Transform playerTranform;
    public string PatrolAnimationName;
    public string ChaseAnimationName;
    public string AttackAnimationName;
    public string DieAnimationName;
    public string IdleAnimationName;

    //Patroling
    [Header("Patrol")]
    [SerializeField] private Vector3 walkPoint;
    [SerializeField] private float maxWalkStopTime = 10f;
    [SerializeField] private float minWalkStopTime = 3f;
    private float walkStopTime;
    private float currentTimer;
    private bool walkPointSet;
    [SerializeField] private float walkPointRange;

    //Attacking
    private bool alreadyAttacked;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    public bool canWalk;
    private Animator animator;
    private bool isAlive = true;

    void Start()
    {
        playerTranform = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!isAlive) return;

        if (GameManager.Instance.IsGameOver || GameManager.Instance.IsGameEnd) return;

        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (playerInSightRange && playerInAttackRange) AttackPlayer();

        if (!canWalk)
        {
            if (playerInSightRange && !playerInAttackRange)
            {
                canWalk = true;
                currentTimer = 0;
                return;
            }
            else if (playerInSightRange && playerInAttackRange)
            {
                canWalk = true;
                currentTimer = 0;
                return;
            }

            animator.Play(IdleAnimationName);
            walkStopTime = UnityEngine.Random.Range(minWalkStopTime, maxWalkStopTime);

            currentTimer += Time.deltaTime;

            if (currentTimer >= walkStopTime)
            {
                canWalk = true;
                currentTimer = 0;
            }

            return;
        }

        if (!playerInSightRange && !playerInAttackRange) Patrolling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
    }

    private void Patrolling()
    {
        if (!canWalk) return;

        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        animator.Play(PatrolAnimationName);
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
            canWalk = false;
        }
    }

    private void SearchWalkPoint()
    {
        float randomZ = UnityEngine.Random.Range(-walkPointRange, walkPointRange);
        float randomX = UnityEngine.Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        animator.Play(ChaseAnimationName);
        agent.SetDestination(playerTranform.position);
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(playerTranform);

        if (!alreadyAttacked)
        {
            animator.Play(AttackAnimationName);
            // Attack code here
            PlayerManager player = playerTranform.GetComponent<PlayerManager>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), attackCD);
        }
    }

    private void ResetAttack()
    {
        animator.Play(IdleAnimationName);
        alreadyAttacked = false;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    public bool IsAlive()
    {
        return isAlive;
    }

    private void Die()
    {
        isAlive = false;
        agent.isStopped = true;
        animator.Play(DieAnimationName);
        Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length + 2f);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
