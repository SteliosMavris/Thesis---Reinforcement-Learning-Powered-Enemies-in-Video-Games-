using UnityEngine;
using UnityEngine.AI;

public class EnemyState : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private int stamina;
    [SerializeField] private int strength;

    [SerializeField] private PlayerState player;
    [SerializeField] private Animator animator;

    [Header("Detection")]
    [SerializeField] private float detectionRange = 15f;
    [SerializeField] private float fieldOfView = 60f;
    private float playerAngle;
    private float distance;
    public AIState currentState = AIState.Patrol;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        distance = 0;
    }

    void Start()
    {
        
    }

    void Update()
    {
        distance = Vector3.Distance(transform.position, player.transform.position);
        Vector3 dirToPlayer = (player.transform.position - transform.position).normalized;
        playerAngle = Vector3.Angle(transform.forward, dirToPlayer);

        if (health <= 0) currentState = AIState.Dead;
        else if (distance > detectionRange) currentState = AIState.Patrol;
        else if(distance <= detectionRange && playerAngle < fieldOfView / 2)
        {
            if (health >= 2) currentState = AIState.Chase;
            else currentState = AIState.Flee;     
        }
    }

    /*
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Hitbox"))
        {
            TakeDamage(PlayerState.Instance.GetStrength());
        }
    }
    */

    public void ReduceHealth(int damage)
    {
        health-=damage;
        if (health <= 0)
        {
            currentState = AIState.Dead;
            Invoke(nameof(Die), 3.5f);
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    public int GetHealth() => health;
    public int GetStrength() => strength;
    public float GetDistance() => distance;
    public AIState GetState() => currentState;
}