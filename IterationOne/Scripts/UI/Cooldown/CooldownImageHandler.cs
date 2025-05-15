using UnityEngine;
using UnityEngine.UI;

public class CooldownImageHandler : MonoBehaviour
{
    [SerializeField]
    private Image cooldownImage;

    private PlayerAttack playerAttack;

    private void Awake()
    {
        playerAttack = FindAnyObjectByType<PlayerAttack>();
    }

    private void OnEnable()
    {
        playerAttack.OnPlayerAttackStart += MakeImageVisible;
        playerAttack.OnPlayerAttackEnd += MakeImageInvisible;
    }

    private void OnDisable()
    {
        playerAttack.OnPlayerAttackStart -= MakeImageVisible;
        playerAttack.OnPlayerAttackEnd -= MakeImageInvisible;
    }

    private void MakeImageVisible()
    {
        cooldownImage.gameObject.SetActive(true);
    }

    private void MakeImageInvisible()
    {
        cooldownImage.gameObject.SetActive(false);
    }
}
