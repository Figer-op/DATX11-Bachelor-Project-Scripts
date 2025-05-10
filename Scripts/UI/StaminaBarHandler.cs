using UnityEngine;
using UnityEngine.UI;

public class StaminaBarHandler : MonoBehaviour
{
    [SerializeField]
    private Slider staminaBar;

    private PlayerBase playerBase; 

    private void Awake()
    {
        playerBase = FindAnyObjectByType<PlayerBase>();
    }

    private void OnEnable()
    {
        playerBase.OnStaminaGained += IncreaseStaminaBar;
        playerBase.OnStaminaLost += DecreaseStaminaBar;
    }

    private void OnDisable()
    {
        playerBase.OnStaminaGained -= IncreaseStaminaBar;
        playerBase.OnStaminaLost -= DecreaseStaminaBar;
    }

    private void IncreaseStaminaBar(float amount)
    {
        staminaBar.value += amount;
    }

    private void DecreaseStaminaBar(float amount)
    {
        staminaBar.value -= amount;
    }
}
