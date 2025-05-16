using UnityEngine;

public class Door : ToggleableSprite
{
    public override void ToggleOn()
    {
        base.ToggleOn();
        gameObject.GetComponent<BoxCollider2D>().isTrigger = IsActive;
    }

    public override void ToggleOff()
    {
        base.ToggleOff();
        gameObject.GetComponent<BoxCollider2D>().isTrigger = IsActive;
    }
}
