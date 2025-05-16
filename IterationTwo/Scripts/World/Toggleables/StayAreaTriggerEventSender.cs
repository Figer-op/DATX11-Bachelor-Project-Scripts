using System.Collections.Generic;
using UnityEngine;

public class StayAreaTriggerEventSender : TriggerEventSender, ITriggerEventSender
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        SendTriggerEvent();
    }
}
