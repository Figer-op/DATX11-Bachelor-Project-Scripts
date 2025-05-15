using UnityEngine;

public class PlayerDash : MonoBehaviour, IHasCooldown
{
    [SerializeField]
    private int force = 3500;

    [SerializeField]
    private Rigidbody2D playerRB; // Important note: Collision detection needs to be continuous.
    [SerializeField]
    private PlayerInput playerInput;
    [SerializeField]
    private PlayerBase player;
    private Vector2 lookDir = Vector2.zero;

    [SerializeField]
    private int abyssLayer;
    [SerializeField]
    private int enemyLayer;
    private bool isLayersExcluded;

    [SerializeField]
    private float staminaCost = 10f;

    [SerializeField]
    private CooldownSO cooldown;

    [SerializeField]
    private CooldownSO layerResetCD;

    public CooldownSO Cooldown
    {
        get { return cooldown; }
    }

    private Camera mainCamera;


    private void Start()
    {
        if (playerInput == null || playerRB == null)
        {
            Debug.LogError("PlayerInput or Rigidbody2D is not assigned!");
            return;
        }

        playerInput.OnDashPressed += HandleDash;

        if (abyssLayer == 0)
        {
            Debug.LogError("There is no abyss layer!");
        }

        if (player == null)
        {
            Debug.LogError("There is no playerBase script assigned to player dash!");
        }

        mainCamera = Camera.main;
    }

    private void Update()
    {
        Vector2 mousePosition = playerInput.Look.ReadValue<Vector2>();
        Vector2 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
        Vector2 currentPosition = transform.position;
        lookDir = worldPosition - currentPosition;
        lookDir = lookDir.normalized;

        LayermaskDelay();
    }

    private void LayermaskDelay()
    {
        if (isLayersExcluded)
        {   
            if (!layerResetCD.IsOnCooldown)
            {   
                playerRB.excludeLayers = 0;
                isLayersExcluded = false;
            }
        }
    }

    private void HandleDash()
    {
        if (lookDir != Vector2.zero && !cooldown.IsOnCooldown && player.CurrentStamina >= staminaCost)
        {
            playerRB.excludeLayers = (1 << abyssLayer) | (1 << enemyLayer);;
            playerRB.AddForce(lookDir * force);
            isLayersExcluded = true;
            player.LoseStamina(staminaCost);
            cooldown.TriggerCooldown();
            layerResetCD.TriggerCooldown();
        }
    }
}
