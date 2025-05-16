using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IdleRandomWander", menuName = "EnemyLogic/IdleLogic/RandomWander")]
public class EnemyIdleRandomWander : EnemyIdleBaseSO
{
    [SerializeField]
    private float movementRange = 5;

    [SerializeField]
    private float movementSpeed = 1;

    private Vector3 targetPosition;
    private Vector3 direction;

    [SerializeField]
    private float resetTime = 5f;
    private float resetTimer;

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        targetPosition = GetRandomPointInCircle();
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        resetTimer -= Time.deltaTime;

        direction = (targetPosition - enemy.transform.position).normalized;
        enemy.Move(direction * movementSpeed);

        if ((enemy.transform.position - targetPosition).sqrMagnitude < 0.01f || resetTimer <= 0)
        {
            targetPosition = GetRandomPointInCircle();
            resetTimer = resetTime;
        }
    }

    private Vector3 GetRandomPointInCircle()
    {
        return enemy.transform.position + ((Vector3)UnityEngine.Random.insideUnitCircle * movementRange);
    }
}
