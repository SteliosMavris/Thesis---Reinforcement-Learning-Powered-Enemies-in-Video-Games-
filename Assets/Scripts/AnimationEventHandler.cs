using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    public bool isAttackingPlayer;
    public bool isBlockingPlayer;
    public bool isAttackingEnemy;
    public bool isBlockingEnemy;
    public static AnimationEventHandler Instance;
    private CharacterController characterController;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        // Set all animation event related variables to their default values
        isAttackingPlayer = false;
        isBlockingPlayer = false;
        isAttackingEnemy = false;
        isBlockingEnemy = false;

        characterController = GetComponentInParent<CharacterController>();
    }

    public void SetIsAttackingPlayer(int value)
    {
        if(value == 1)
            isAttackingPlayer = true;
        else if(value == 0)
            isAttackingPlayer = false;  
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
            isAttackingEnemy = false;  
    }

    public void SetIsBlockingEnemy(int value)
    {
        if(value == 1)
            isBlockingEnemy = true;
        else if(value == 0)
            isBlockingEnemy = false;
    }

    public void OnAttackAnimationFinished()
    {
        Debug.Log("Finished Attack");
        isAttackingPlayer = false;
        if (characterController != null)
            characterController.enabled = true;
        PlayerController.Instance.SetCanSwapWeapons(true);
    }

    public bool GetIsAttackingPlayer() => isAttackingPlayer;
    public bool GetIsBlockingPlayer() => isBlockingPlayer;
    public bool GetIsAttackingEnemy() => isAttackingEnemy;
    public bool GetIsBlockingEnemy() => isBlockingEnemy;
}
