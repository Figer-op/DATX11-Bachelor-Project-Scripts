using UnityEngine;

public class PlayerStartSetterToggleable : AbstractToggleable
{
    [SerializeField]
    private Vector2 newPlayerPosition;

    public override void ToggleOff()
    {
        base.ToggleOff();
        PlayerBase player = FindAnyObjectByType<PlayerBase>();

        if (player == null)
        {
            Debug.LogError("Could not find player!");
            return;
        }

        player.StartPosition = newPlayerPosition;
    }
}
