using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private static readonly int DieHash = Animator.StringToHash("Die");
    private static readonly int AttackHash = Animator.StringToHash("Attack");
    private static readonly int SpeedHash = Animator.StringToHash("Speed");

    [SerializeField] public Transform[] waypoints;
    [SerializeField] private GameObject player;

    [Header("Speed Settings")]
    [SerializeField] private float patrolSpeed = 1f;
    [SerializeField] private float chaseSpeed = 4f;
    [SerializeField] private float fleeSpeed = 4f;

    [Header("Combat")]
    [SerializeField] private float meleeRange = 1f;
    [SerializeField] private float attackSpeed = 1;
    [SerializeField] private float attackCooldown = 2.5f;
    public AIState currentState;
    private EnemyState enemyState;
    private NavMeshAgent agent;
    //private CharacterController characterController;
    private PlayerState playerState;
    private Animator animator;
    private int destPoint = 0;
    private float nextAttackTime;
    private float distance;
    private bool leftPatrol = false;
    private bool isAttacking = false;
    private CharacterController characterController;
    public string AttackSpeedHash { get; private set; }

    void Awake()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        playerState = player.GetComponent<PlayerState>();
        characterController = gameObject.GetComponent<CharacterController>();
    }

    void Start()
    {
        enemyState = gameObject.GetComponent<EnemyState>();
        animator = gameObject.GetComponentInChildren<Animator>();
        SetNextWaypoint();
    }

    // Update is called once per frame
    void Update()
    {   
        if(isAttacking)
            return;
        
        currentState = enemyState.GetState();
        
        if(currentState == AIState.Dead)
        {
            animator.SetFloat(SpeedHash, 0);
            animator.SetTrigger(DieHash);
            return;
        }

        if (playerState.GetHealth() <= 0)
        {
            animator.SetFloat(SpeedHash, 0);
            return;
        }
        

        switch (currentState)
        {
            case AIState.Patrol:
                agent.speed = patrolSpeed;
                animator.SetFloat(SpeedHash, agent.speed);
                Patrol();
                break;
            case AIState.Chase:
                agent.speed = chaseSpeed;
                animator.SetFloat(SpeedHash, agent.speed);
                Chase();
                break;
            case AIState.Flee:
                agent.speed = fleeSpeed;
                animator.SetFloat(SpeedHash, agent.speed);
                Flee();
                break;
        }
    }
    void Patrol()
    {
        if (leftPatrol)
        {
            agent.ResetPath();
            SetNextWaypoint();
        }
        else if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            SetNextWaypoint();
        }
    }
    void Chase()
    {
        leftPatrol = true;
        agent.SetDestination(player.transform.position);
        if (Time.time >= nextAttackTime)
        {
            if (distance <= meleeRange && playerState.GetHealth() > 0)
            {
                animator.SetTrigger(AttackHash);
                Attack();
                nextAttackTime = Time.time + attackCooldown;
            }
        }
    }
    void Flee()
    {
        leftPatrol = true;
        Vector3 fleeDir = transform.position - player.transform.position;
        Vector3 destination = transform.position + fleeDir.normalized * 10;
        agent.SetDestination(destination);
    }

    public void Attack()
    {
        Debug.Log("Attacking");
        if (isAttacking)
            return;
        
        characterController.enabled = false;
        
        agent.speed = 0;

        isAttacking = true;

        animator.SetFloat(SpeedHash, 0f);

        animator.SetFloat(AttackSpeedHash, attackSpeed);

        animator.SetTrigger(AttackHash);

        playerState.ReduceHealth(enemyState.GetStrength());
        
        Invoke(nameof(FinishAttack), attackCooldown);

    }

    private void FinishAttack()
    {
        isAttacking = false;
    }

    void Die()
    {
        gameObject.SetActive(false);
    }

    void SetNextWaypoint()
    {
        if (waypoints.Length == 0) return;

        // If the enemy has chased the player before entering this method, re-route to nearest waypoint 
        if (leftPatrol)
        {
            agent.SetDestination(FindNearestWaypoint().position);
            leftPatrol = false;
        }
        // Otherwise just go to next expected waypoint
        else
        {
            agent.SetDestination(waypoints[destPoint].position);
        }

        destPoint = (destPoint + 1) % waypoints.Length;   
    }

    private Transform FindNearestWaypoint()
    {
        float minDist = Mathf.Infinity;
        int minWPIndx = 0;
        for (int i = 0; i < waypoints.Length; i++)
        {
            float currentDistance = Vector3.Distance(transform.position, waypoints[i].position);
            if (currentDistance < minDist)
            {
                minWPIndx = i;
                minDist = currentDistance;
            }
        }
        destPoint = minWPIndx % waypoints.Length;
        return waypoints[minWPIndx];
    }

}
