using System.Collections.Generic;
using UnityEngine;

public class InteractionHandler : MonoBehaviour
{
    [SerializeField]
    private PlayerInput interactInput;
    private List<IInteractable> interactables = new ();
    private IInteractable closestInteractable;

    private void Start()
    {
        interactInput.OnInteractPressed += HandleInteraction;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        var interaction = collision.GetComponent<IInteractable>();
        if (interaction == null)
        {
            return;
        }
        interactables.Add(interaction);
    }

    private void FixedUpdate()
    {
        if (closestInteractable == null && interactables.Count > 0)
        {
            closestInteractable = interactables[0];
            closestInteractable.ShowInteractableUI();
        }
        foreach (var interactable in interactables)
        {
            if (interactable == null)
            {
                interactables.Remove(interactable);
                continue;
            }
            var interactableMono = interactable as MonoBehaviour;
            var closestInteractableMono = closestInteractable as MonoBehaviour;
            if (closestInteractableMono == null || interactableMono == null)
            {
                continue;
            }
            if (Vector2.Distance(transform.position, interactableMono.transform.position) < Vector2.Distance(transform.position, closestInteractableMono.transform.position))
            {
                closestInteractable.HideInteractableUI();
                closestInteractable = interactable;
                closestInteractable.ShowInteractableUI();
            }
        }
        if (closestInteractable != null && interactables.Count == 0)
        {
            closestInteractable.HideInteractableUI();
            closestInteractable = null;
        }
        interactables.Clear();
    }

    private void HandleInteraction()
    {
        interactables.Clear();
        if (closestInteractable == null)
        {
            return;
        }
        closestInteractable.HideInteractableUI();
        closestInteractable.OnInteract();
        closestInteractable = null;
    }
}
