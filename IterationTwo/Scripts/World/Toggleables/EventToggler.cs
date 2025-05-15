using System.Collections.Generic;
using UnityEngine;

public class EventToggler : Toggler
{
    [SerializeField]
    private List<string> triggeredBy;

    private void Start()
    {
        TriggerEventSender.OnTrigger += OnToggleEvent;
    }

    private void OnToggleEvent(List<string> triggers)
    {
        foreach (var trigger in triggers)
        {
            if (triggeredBy.Contains(trigger))
            {
                Toggle();
                break;
            }
        }
    }

    private void OnDestroy()
    {
        TriggerEventSender.OnTrigger -= OnToggleEvent;
    }
}
