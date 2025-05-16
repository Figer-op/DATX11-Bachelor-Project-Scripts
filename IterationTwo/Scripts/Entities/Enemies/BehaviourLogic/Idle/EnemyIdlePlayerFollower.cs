using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IdlePlayerFollower", menuName = "EnemyLogic/IdleLogic/PlayerFollower")]
public class EnemyIdlePlayerFollower : EnemyIdleBaseSO
{
    [SerializeField]
    private float movementSpeed = 1;

    private Vector2 targetPosition;
    private Vector2 direction;
    private bool hasExitedAtLeastOnce;

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        targetPosition = hasExitedAtLeastOnce && playerTransform != null ? playerTransform.position : enemy.transform.position;
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();
        Vector2 currentPosition = enemy.transform.position;
        if ((currentPosition - targetPosition).sqrMagnitude < 0.01f)
        {
            enemy.Move(Vector2.zero);
            return;
        }
        direction = (targetPosition - currentPosition).normalized;
        enemy.Move(direction * movementSpeed);
    }

    public override void DoExitLogic()
    {
        hasExitedAtLeastOnce = true;
        base.DoExitLogic();
    }
}
