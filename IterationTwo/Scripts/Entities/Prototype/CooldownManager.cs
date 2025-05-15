using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
public class CooldownManager : MonoBehaviour
{
    public static CooldownManager Instance { get; private set; }

    private List<CooldownSO> cooldowns = new();

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("Found more than one Cooldown Manager in scene, destroying newest.");
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);

        cooldowns = Resources.LoadAll<CooldownSO>("Cooldowns").ToList();

        foreach (var cd in cooldowns)
        {
            if (cd.name.IndexOf("player", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                cd.OnCooldownChanged += HandleCooldownChanged;
            }
        }
    }

    private void Update()
    {
        foreach (var cd in cooldowns)
        {
            cd.OnUpdateLogic();
        }
    }

    private void HandleCooldownChanged(string cdName, float cooldown)
    {
        //TODO: Mr.UI add UI logic 
        //Debug.Log(cdName + ": " + cooldown);
    }
}
