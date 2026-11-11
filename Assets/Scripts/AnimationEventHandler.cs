using System.Collections;
using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    public bool isAttacking;
    public bool isBlocking;
    public static AnimationEventHandler Instance;
    private CharacterController characterController;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        // Set all animation event related variables to their default values
        isAttacking = false;
        isBlocking = false;

        characterController = GetComponentInParent<CharacterController>();
    }

    public void SetIsAttacking(int value)
    {
        if(value == 1)
            isAttacking = true;
        else if(value == 0)
            isAttacking = false;  
    }

    public void SetIsBlocking(int value)
    {
        if(value == 1)
            isBlocking = true;
        else if(value == 0)
            isBlocking = false;
    }

    public void OnAttackAnimationFinished()
    {
        Debug.Log("Finished Attack");
        isAttacking = false;
        if (characterController != null)
            characterController.enabled = true;
        PlayerController.Instance.SetCanSwapWeapons(true);
    }

    public bool GetIsAttacking() => isAttacking;
    public bool GetIsBlocking() => isBlocking;
}
