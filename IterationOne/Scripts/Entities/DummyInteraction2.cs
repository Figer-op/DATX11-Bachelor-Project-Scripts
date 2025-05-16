using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyInteraction2 : MonoBehaviour, IInteractable
{
    [SerializeField]
    private GameObject interactionUI;
    public void OnInteract()
    {
        Debug.Log("Interacted with dummy 2");
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