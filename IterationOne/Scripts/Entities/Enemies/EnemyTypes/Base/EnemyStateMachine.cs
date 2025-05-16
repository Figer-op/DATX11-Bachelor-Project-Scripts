using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine
{
    public EnemyBehaviourBaseSO CurrentEnemyState { get; private set; }

    public EnemyStateMachine(EnemyBehaviourBaseSO CurrentEnemyState)
    {
        this.CurrentEnemyState = CurrentEnemyState;
        CurrentEnemyState.DoEnterLogic();
    }

    public void ChangeState(EnemyBehaviourBaseSO newState)
    {
        CurrentEnemyState.DoExitLogic();
        CurrentEnemyState = newState;
        CurrentEnemyState.DoEnterLogic();
    }
}
