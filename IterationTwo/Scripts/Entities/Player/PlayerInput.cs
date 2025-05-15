using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private InputSystem_Actions playerControls;

    public InputAction Move { get; private set; }
    public InputAction Interact { get; private set; }
    public InputAction Attack { get; private set; }
    public InputAction Look { get; private set; }
    public InputAction EscapeMenu { get; private set; }

    public event System.Action OnInteractPressed;
    public event System.Action OnDashPressed;
    public event System.Action<int> OnAttackPressed;
    public event System.Action OnEscapePressed;

    private void Awake()
    {
        playerControls = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        Move = playerControls.Player.Move;
        Interact = playerControls.Player.Interact;
        Attack = playerControls.Player.Attack;
        Look = playerControls.Player.Look;
        EscapeMenu = playerControls.UI.EscapeMenu;
        playerControls.Enable();

        playerControls.Player.Interact.performed += HandleInteract;
        playerControls.Player.Dash.performed  += HandleDash;
        playerControls.Player.Attack.performed += HandleAttack;
        playerControls.Player.SpecialAttack.performed += HandleSpecialAttack;
        playerControls.UI.EscapeMenu.performed += HandleEscape;
    }

    private void OnDisable()
    {
        playerControls.Disable();
        playerControls.Player.Interact.performed -= HandleInteract;
        playerControls.Player.Dash.performed  -= HandleDash;
        playerControls.Player.Attack.performed -= HandleAttack;
        playerControls.Player.SpecialAttack.performed -= HandleSpecialAttack;
        playerControls.UI.EscapeMenu.performed -= HandleEscape;
    }

    private void HandleInteract(InputAction.CallbackContext context)
    {
        OnInteractPressed?.Invoke();
    }
    private void HandleDash(InputAction.CallbackContext context)
    {
        OnDashPressed?.Invoke(); 
    }
    private void HandleAttack(InputAction.CallbackContext context)
    {
        OnAttackPressed?.Invoke(0); // for now we are treating weapon's 1st abilities as normal abilities
    }
    
    private void HandleSpecialAttack(InputAction.CallbackContext context)
    {
        OnAttackPressed?.Invoke(1); // for now we are treating weapon's 2nd abilities as special abilities
    }

    private void HandleEscape(InputAction.CallbackContext context) {
        OnEscapePressed?.Invoke();
    }
}
