using UnityEngine;

public abstract class TargetCheck : MonoBehaviour
{
    protected GameObject target;
    protected EnemyBase enemy;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        enemy = GetComponentInParent<EnemyBase>();
    }
}
