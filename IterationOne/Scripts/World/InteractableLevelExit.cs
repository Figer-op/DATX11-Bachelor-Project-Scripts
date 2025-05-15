using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class InteractableLevelExit : MonoBehaviour, IInteractable
{
    [SerializeField]
    protected string levelName;

    [SerializeField]
    private GameObject interactionUI;

    [SerializeField]
    private DungeonType nextDungeonType;

    private bool wasDestroyed = false;

    protected virtual void Start()
    {
        // Load interaction UI addressable
        var interactionUIPrefab = Addressables.LoadAssetAsync<GameObject>("Assets/Main/Prefabs/UI/Prompts/InteractionPrompt.prefab");
        // instantiate prompt
        interactionUIPrefab.Completed += handle =>
        {
            if (wasDestroyed || this == null)
            {
                return;
            }
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
        // Save the game data before loading the new scene
        GameDataPersistenceManager.Instance.SaveTheData();

        if (TryGetComponent<DestroyableObject>(out var destroyable))
        {
            ObjectTracker.DestroyedObjectIDs.Add(destroyable.ObjectID);
        }

        SceneManager.LoadScene(levelName);
    }

    public void ShowInteractableUI()
    {
        interactionUI.SetActive(true);
    }

    public void HideInteractableUI()
    {
        interactionUI.SetActive(false);
    }
    private void OnDestroy()
    {
        wasDestroyed = true;
    }
}
