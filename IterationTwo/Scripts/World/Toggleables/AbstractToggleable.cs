using NUnit.Framework;
using UnityEngine;

public abstract class AbstractToggleable : MonoBehaviour, IToggleable
{
    [field:SerializeField]
    public bool IsActive {get; private set;} = false;

    public virtual void Toggle()
    {
        if (IsActive)
                {
                    ToggleOff();
                }
                else
                {
                    ToggleOn();
                }
    }

    public virtual void ToggleOn()
    {
        IsActive = true;
    }

    public virtual void ToggleOff()
    {
        IsActive = false;
    }

    private void OnValidate()
    {
        if (IsActive)
        {
            ToggleOn();
        }
        else
        {
            ToggleOff();
        }
    }
}
