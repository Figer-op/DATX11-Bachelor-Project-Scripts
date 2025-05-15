using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractChest : ToggleableSprite
{   
    public event Action OnChestSpawned;

    private void Start()
    {
        OnChestSpawned?.Invoke();
    }
}