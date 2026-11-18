using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    public bool isAttackingPlayer;
    public bool isBlockingPlayer;
    public bool isAttackingEnemy;
    public bool isBlockingEnemy;
    public static AnimationEventHandler Instance;
    [SerializeField] private CharacterController characterControllerPlayer;
    [SerializeField] private CharacterController characterControllerEnemy;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        isAttackingPlayer = false;
        isBlockingPlayer = false;
        isAttackingEnemy = false;
        isBlockingEnemy = false;
    }

    public void SetIsAttackingPlayer(int value)
    {
        if(value == 1)
            isAttackingPlayer = true;
        else if(value == 0)
        {
            isAttackingPlayer = false;
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            AttackDetection attackDetection = player.GetComponentInChildren<AttackDetection>();
            AttackDetection.ResetAttack(attackDetection);
        } 
    }

    public void SetIsBlockingPlayer(int value)
    {
        if(value == 1)
            isBlockingPlayer = true;
        else if(value == 0)
            isBlockingPlayer = false;
    }

    public void SetIsAttackingEnemy(int value)
    {
        if(value == 1)
            isAttackingEnemy = true;
        else if(value == 0)
        {
            isAttackingEnemy = false;
            GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
            AttackDetection attackDetection = enemy.GetComponentInChildren<AttackDetection>();
            AttackDetection.ResetAttack(attackDetection);
        }
    }

    public void SetIsBlockingEnemy(int value)
    {
        if(value == 1)
            isBlockingEnemy = true;
        else if(value == 0)
            isBlockingEnemy = false;
    }

    public void OnAttackAnimationFinishedPlayer()
    {
        Debug.Log("Finished Attack");
        isAttackingPlayer = false;
        if (characterControllerPlayer != null)
            characterControllerPlayer.enabled = true;
        PlayerController.Instance.SetCanSwapWeapons(true);
    }

    public void OnAttackAnimationFinishedEnemy()
    {
        Debug.Log("Finished Attack");
        isAttackingEnemy = false;
        if (characterControllerEnemy != null)
            characterControllerEnemy.enabled = true;
    }

    public bool GetIsAttackingPlayer() => isAttackingPlayer;
    public bool GetIsBlockingPlayer() => isBlockingPlayer;
    public bool GetIsAttackingEnemy() => isAttackingEnemy;
    public bool GetIsBlockingEnemy() => isBlockingEnemy;
}
