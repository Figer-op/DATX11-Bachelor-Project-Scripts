using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "EnemyPathfindingChase", menuName = "EnemyLogic/ChaseLogic/PathfindingChase")]

public class EnemyPathfindingChase : EnemyChaseBaseSO
{
    [SerializeField]
    private float movementSpeed = 1.75f;

    private TilemapPathfinder pathfinder;

    private Stack<Vector3> path;

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();

        Tilemap tilemap = GameObject.FindAnyObjectByType<Tilemap>();
        pathfinder = new TilemapPathfinder(tilemap);
        enemy.StartCoroutine(FindPath());
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        if (path == null)
        {
            return;
        }

        Vector2 moveDirection = (path.Peek() - enemy.transform.position).normalized;
        enemy.Move(moveDirection * movementSpeed);
    }

    private IEnumerator FindPath()
    {
        while (playerTransform != null)
        {
            path = pathfinder.FindPath(enemy.transform.position, playerTransform.position);
            yield return null;
        }
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
        enemy.StopCoroutine(FindPath());
    }
}
