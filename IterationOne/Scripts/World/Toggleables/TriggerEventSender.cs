using System.Collections.Generic;
using UnityEngine;

public class TriggerEventSender : MonoBehaviour, ITriggerEventSender
{
    [SerializeField]
    private List<string> triggers;

    public static event System.Action<List<string>> OnTrigger;

    public void SendTriggerEvent()
    {
        OnTrigger?.Invoke(triggers);
    }
}
