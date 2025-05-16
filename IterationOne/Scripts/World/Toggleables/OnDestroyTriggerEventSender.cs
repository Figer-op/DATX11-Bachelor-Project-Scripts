using UnityEngine;

public class OnDestroyTriggerEventSender: TriggerEventSender
{
    private void OnDestroy()
    {
        if (TryGetComponent<EnemyBase>(out var enemyBase))
        {
            if (enemyBase.CurrentHealth > 0)
            {
                return;
            }
        }

        SendTriggerEvent();
    }
}
