using UnityEngine;

public class EnemyIdleBaseSO : EnemyBehaviourBaseSO
{
    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        if (enemy.IsAggroed)
        {
            enemy.StateMachine.ChangeState(enemy.EnemyChaseBaseInstance);
        }
    }
}
