using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InputScript : MonoBehaviour
{
    private InputSystem_Actions playerControls;
    public InputAction EscapeMenu { get; private set; }
    public event System.Action OnEscapePressed;

    private void Awake()
    {
        playerControls = new InputSystem_Actions();
    }
    private void OnEnable()
    {
        EscapeMenu = playerControls.UI.EscapeMenu;
        playerControls.Enable();
        playerControls.UI.EscapeMenu.performed += HandleEscape;
        
    }
    private void OnDisable()
    {
        playerControls.Disable();
        playerControls.UI.EscapeMenu.performed -= HandleEscape;
    }

    private void HandleEscape(InputAction.CallbackContext context) {
        SceneManager.LoadScene(0);
    }
}
