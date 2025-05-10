using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarHandler : MonoBehaviour
{
    [SerializeField]
    private Slider healthBar;

    [SerializeField]
    private EnemyBase enemyBase;

    private void Awake()
    {
        healthBar.maxValue = enemyBase.MaxHealth;
        healthBar.value = healthBar.maxValue;
    }

    private void OnEnable()
    {
        enemyBase.OnBeingAttacked += DecreaseHealthBar;
    }

    private void OnDisable()
    {
        enemyBase.OnBeingAttacked -= DecreaseHealthBar;
    }

    private void DecreaseHealthBar(float amount)
    {
        healthBar.value -= amount;
    }
}
