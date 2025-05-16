using UnityEngine;

public class EnemyChaseBaseSO : EnemyBehaviourBaseSO
{
    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        if (enemy.IsWithinStrikingDistance)
        {
            enemy.StateMachine.ChangeState(enemy.EnemyAttackBaseInstance);
        }
        else if (!enemy.IsAggroed || playerTransform == null)
        {
            enemy.StateMachine.ChangeState(enemy.EnemyIdleBaseInstance);
        }
    }
}
