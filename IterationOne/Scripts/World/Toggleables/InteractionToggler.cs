using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class InteractionToggler : Toggler, IInteractable
{
    [SerializeField]
    private Collider2D interactionTrigger;
    [SerializeField]
    private GameObject interactionUI;

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
    }

    public void OnInteract()
    {
        Toggle();
        if (onlyOnce)
        {
            interactionTrigger.enabled = false;
        }
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
