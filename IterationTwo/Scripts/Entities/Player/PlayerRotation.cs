using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRotation : MonoBehaviour
{
    [SerializeField]
    private PlayerInput playerInput;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        Vector2 mousePosition = playerInput.Look.ReadValue<Vector2>();
        Vector2 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
        Vector2 currentPosition = transform.position;
        Vector2 positionDifference = worldPosition - currentPosition;

        float rotationAngle = Mathf.Atan2(positionDifference.y, positionDifference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotationAngle);
    }
}
