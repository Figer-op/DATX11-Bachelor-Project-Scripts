using UnityEngine;

public class ToggleableSprite : AbstractToggleable
{
    [SerializeField]
    private Sprite passiveSprite;
    [SerializeField]
    private Sprite activeSprite;

    public override void ToggleOn()
    {
        base.ToggleOn();
        if (activeSprite != null && passiveSprite != null)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = activeSprite;
        }
    }

    public override void ToggleOff()
    {
        base.ToggleOff();
        if (activeSprite != null && passiveSprite != null)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = passiveSprite;
        }
    }
}
