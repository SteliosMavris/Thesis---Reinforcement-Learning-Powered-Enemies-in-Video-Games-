using Unity.VisualScripting;
using UnityEngine;

public class AttackDetection : MonoBehaviour
{
    public bool appliedDamage;
    public bool damageBlocked;
    void Awake()
    {
        appliedDamage = false;
        damageBlocked = false;
    }

    void Start()
    {
        
    }

    
    private void OnTriggerStay(Collider other)
    {
        string targetTag = "";
        bool isAttacking = false;

        // Identify attacker
        if (GetComponentInParent<PlayerState>() != null)
        {
            targetTag = "Enemy";
            isAttacking = AnimationEventHandler.Instance.GetIsAttackingPlayer();
        }
        else if (GetComponentInParent<EnemyState>() != null)
        {
            targetTag = "Player";
            isAttacking = AnimationEventHandler.Instance.GetIsAttackingEnemy();
        }

        if (!isAttacking || damageBlocked || appliedDamage)
            return;

        // Check shieldbox
        if (other.CompareTag("Shieldbox"))
        {
            bool isBlocking = false;

            if (other.GetComponentInParent<PlayerState>() != null)
                isBlocking = AnimationEventHandler.Instance.GetIsBlockingPlayer();

            else if (other.GetComponentInParent<EnemyState>() != null)
                isBlocking = AnimationEventHandler.Instance.GetIsBlockingEnemy();

            if (isBlocking)
            {
                damageBlocked = true;
                Debug.Log("Attack Blocked!");
            }

            return;
        }

        // Check hurtbox
        if (other.CompareTag(targetTag) && other is CapsuleCollider)
        {
            if (targetTag == "Enemy")
            {
                EnemyState enemy = other.GetComponentInParent<EnemyState>();

                if (enemy != null)
                {
                    enemy.ReduceHealth(WeaponManager.Instance.GetWeaponDamage());
                    Debug.Log("Attacked Enemy");
                }
            }
            else if (targetTag == "Player")
            {
                PlayerState player = other.GetComponentInParent<PlayerState>();
                EnemyState enemy = GetComponentInParent<EnemyState>();

                if (player != null && enemy != null)
                {
                    player.ReduceHealth(enemy.GetStrength());
                    Debug.Log("Attacked Player");
                }
            }

            appliedDamage = true;
        }
    }

    public static void ResetAttack(AttackDetection thisGameobject)
    {
        thisGameobject.appliedDamage = false;
        thisGameobject.damageBlocked = false;
        Debug.Log("Reset Attack");
    }

}