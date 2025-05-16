using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IdleGoBacker", menuName = "EnemyLogic/IdleLogic/GoBacker")]
public class EnemyIdleGoBacker: EnemyIdleBaseSO
{
    [SerializeField]
    private float movementSpeed = 1;

    private Vector2 targetPosition;
    private Vector2 direction;
    private bool isTargetPositionIntialized = false;

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        if (!isTargetPositionIntialized)
        {
            targetPosition = enemy.transform.position;
            isTargetPositionIntialized = true;
        }
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
        targetPosition = enemy.transform.position;
        base.DoExitLogic();
    }
}
