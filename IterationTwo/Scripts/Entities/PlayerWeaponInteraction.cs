using UnityEngine;
using UnityEngine.AddressableAssets;

public class PlayerWeaponInteraction : MonoBehaviour, IInteractable
{
    private PlayerBase playerBase;
    [SerializeField]
    private GameObject interactionUI;

    // I don't think this should be in OnEnable() because I don't see a scenario where a weapon is being enabled before the player class exists
    //  unless we want populate the dungeon with weapons laying around in the map, disable them, then bring the player to the scene.
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
    }
    public void OnInteract()
    {
        playerBase.EquipNewWeapon(gameObject);
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
