using UnityEngine;
using UnityEngine.UI;

public class HealthBarHandler : MonoBehaviour
{
    [SerializeField]
    private Slider healthBar;
    private PlayerBase playerBase; 

    private void Awake()
    {
        playerBase = FindAnyObjectByType<PlayerBase>();
    }

    private void Start()
    {
        healthBar.value = playerBase.CurrentHealth;
    }

    private void OnEnable()
    {
        playerBase.OnBeingHealed += IncreaseHealthBar;
        playerBase.OnBeingAttacked += DecreaseHealthBar;
    }
    private void OnDisable()
    {
        playerBase.OnBeingHealed -= IncreaseHealthBar;
        playerBase.OnBeingAttacked -= DecreaseHealthBar;
    }

    private void IncreaseHealthBar(float amount)
    {
        healthBar.value += amount;
    }

    private void DecreaseHealthBar(float amount)
    {
        healthBar.value -= amount;
    }
}
