using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;


public class PlayerState : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private int stamina;
    private CharacterController characterController;
    public static PlayerState Instance;
    
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        characterController = gameObject.GetComponent<CharacterController>();
    }
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    

    private void TakeDamage(int damage)
    {
        health -= damage;

        if(health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        gameObject.SetActive(false);

        //GameManager.Instance.SetState("lose");
        //PlayerPrefs.SetString("result", "YOU DIED!");
    }
    public void ReduceHealth(int damage)
    {
        health-=damage;
        if (health <= 0)
        {
            characterController.enabled = false;
            WeaponManager.Instance.GetWeaponAnimator().SetTrigger("Die");

            //ParticleSystem psDeath = Instantiate(deathParticleSystem, this.transform.position, Quaternion.identity);
            //Destroy(psDeath, 1f);

            Invoke(nameof(Die), 3.5f);
        }
    }
    public int GetHealth() => health;
    public int GetStamina() => stamina;
}