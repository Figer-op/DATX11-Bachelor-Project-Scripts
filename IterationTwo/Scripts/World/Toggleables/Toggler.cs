using System.Collections.Generic;
using UnityEngine;

public class Toggler : MonoBehaviour, IToggler
{
    [SerializeField]
    private List<AbstractToggleable> toggleables;
    [SerializeField]
    private ToggleType toggleType;
    [SerializeField]
    protected bool onlyOnce = false;
    protected bool hasBeenTriggered = false;

    private enum ToggleType
    {
        Toggle,
        ToggleOn,
        ToggleOff
    }

    public virtual void Toggle()
    {
        if (onlyOnce && hasBeenTriggered)
        {
            return;
        }
        hasBeenTriggered = true;
        foreach (var toggleable in toggleables)
        {
            switch (toggleType)
            {
                case ToggleType.Toggle:
                    toggleable.Toggle();
                    break;
                case ToggleType.ToggleOn:
                    toggleable.ToggleOn();
                    break;
                case ToggleType.ToggleOff:
                    toggleable.ToggleOff();
                    break;
            }
        }
    }
}
