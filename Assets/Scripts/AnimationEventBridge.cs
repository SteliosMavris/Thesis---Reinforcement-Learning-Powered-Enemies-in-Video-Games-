
using UnityEngine;

public class AnimationEventBridge : MonoBehaviour
{
    public void SetIsAttackingPlayer(int value)
    {
        AnimationEventHandler.Instance.SetIsAttackingPlayer(value);
    }

    public void SetIsBlockingPlayer(int value)
    {
        AnimationEventHandler.Instance.SetIsBlockingPlayer(value);
    }

    public void SetIsAttackingEnemy(int value)
    {
        AnimationEventHandler.Instance.SetIsAttackingEnemy(value);
    }

    public void SetIsBlockingEnemy(int value)
    {
        AnimationEventHandler.Instance.SetIsBlockingEnemy(value);
    }

    public void OnAttackAnimationFinishedPlayer()
    {
        AnimationEventHandler.Instance.OnAttackAnimationFinishedPlayer();
    }

    public void OnAttackAnimationFinishedEnemy()
    {
        AnimationEventHandler.Instance.OnAttackAnimationFinishedEnemy();
    }
}
