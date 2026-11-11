using UnityEngine;

public class AttackDetection : MonoBehaviour
{
    private bool appliedDamage;
    void Awake()
    {
        appliedDamage = false;
    }

    void Start()
    {
        
    }

    void OnTriggerStay(Collider other)
        {
            // Detect for enemy weapon collisions and inflict damage
            if(AnimationEventHandler.Instance.GetIsAttacking() == true 
                && other.gameObject.CompareTag("Enemy")
                && other.GetType() == typeof(CapsuleCollider) 
                && !appliedDamage)
            {                   
                
                EnemyState enemy = other.gameObject.GetComponent<EnemyState>();
                Debug.Log("Detected Attack!");
                enemy.TakeDamage(WeaponManager.Instance.GetWeaponDamage());
                appliedDamage = true;
            }
        }
    void OnTriggerExit(Collider other)
    {
        appliedDamage = false;
    }
}