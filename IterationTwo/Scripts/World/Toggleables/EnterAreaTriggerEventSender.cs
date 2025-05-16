using System.Collections.Generic;
using UnityEngine;

public class EnterAreaTriggerEventSender : TriggerEventSender, ITriggerEventSender
{
    [SerializeField]
    private bool onlyOnce = false;
    private bool hasTriggered = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (onlyOnce && hasTriggered)
        {
            return;
        }
        SendTriggerEvent();
        hasTriggered = true;
    }
}
