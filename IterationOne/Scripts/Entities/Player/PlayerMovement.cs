using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private PlayerInput playerInput;
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private float movementSpeed = 5f;

    private Vector2 moveDir = Vector2.zero;

    private void Update()
    {
        moveDir = playerInput.Move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveDir * movementSpeed;
    }
}
