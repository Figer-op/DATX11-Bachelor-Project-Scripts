using UnityEngine;

public class MenuHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject escapeMenu;
    private PlayerInput playerInput; 
    private void Awake() 
    {
        playerInput = FindAnyObjectByType<PlayerInput>();
    }

    private void OnEnable()
    {
        playerInput.OnEscapePressed += ToggleEscapeMenu;
    }

    private void OnDisable()
    {
        playerInput.OnEscapePressed -= ToggleEscapeMenu;
    }

    private void ToggleEscapeMenu() 
    {
        escapeMenu.SetActive(!escapeMenu.activeSelf);
    }
}
