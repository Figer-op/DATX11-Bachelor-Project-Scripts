using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChaseDirectChase", menuName = "EnemyLogic/ChaseLogic/DirectChase")]

public class EnemyChaseDirectToPlayer : EnemyChaseBaseSO
{
    [SerializeField]
    private float movementSpeed = 1.75f;

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();
        // Have to check here as well because when there are multiple enemies,
        // there can be race conditions
        if (playerTransform == null)
        {
            return;
        }
        Vector2 moveDirection = (playerTransform.position - enemy.transform.position).normalized;

        enemy.Move(moveDirection * movementSpeed);
    }
}
