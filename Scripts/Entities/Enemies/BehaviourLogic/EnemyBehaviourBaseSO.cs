using UnityEngine;

public class EnemyBehaviourBaseSO : ScriptableObject
{
    protected EnemyBase enemy;

    protected Transform playerTransform;

    public virtual void Initialize(EnemyBase enemy)
    {
        this.enemy = enemy;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

    }

    public virtual void DoEnterLogic() { }
    public virtual void DoExitLogic() { ResetValues(); }
    public virtual void DoFrameUpdateLogic() { }
    public virtual void DoPhysicsLogic() { }
    public virtual void DoAnimationTriggerEventLogic(EnemyBase.AnimationTriggerType triggerType) { }

    // Maybe needed, for when you need to reset values between states. 
    public virtual void ResetValues() { }
}
