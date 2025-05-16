using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class HealthPotionInteraction : MonoBehaviour, IInteractable
{
    [SerializeField]
    private int healAmount;

    private PlayerBase playerBase;

    [SerializeField]
    private GameObject interactionUI;

    public event Action OnHealthPotionSpawned;

    private void Start() 
    {
        // Load interaction UI addressable
        var interactionUIPrefab = Addressables.LoadAssetAsync<GameObject>("Assets/Main/Prefabs/UI/Prompts/InteractionPrompt.prefab");
        // instantiate prompt
        interactionUIPrefab.Completed += handle =>
        {
            if (handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
            {
                interactionUI = Instantiate(handle.Result);
                interactionUI.transform.SetParent(transform, false); // Set the parent to this object
                interactionUI.SetActive(false);
            }
            else
            {
                Debug.LogError("Failed to load interaction UI prefab.");
            }
        };
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        if (!player.TryGetComponent<PlayerBase>(out playerBase))
        {
            Debug.LogWarning("Player object not found or missing PlayerBase component!");
        }

        OnHealthPotionSpawned?.Invoke();
    }

    public void OnInteract()
    {
        playerBase.Heal(healAmount);
        Destroy(gameObject);
    }
    public void ShowInteractableUI()
    {
        interactionUI.SetActive(true);
    }

    public void HideInteractableUI()
    {
        interactionUI.SetActive(false);
    }
}
