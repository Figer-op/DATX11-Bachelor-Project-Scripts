using System.Collections.Generic;
using UnityEngine;

public class Chest : AbstractChest
{
    [SerializeField]
    private List<GameObject> items;
    public override void ToggleOn()
    {
        base.ToggleOn();
        foreach (GameObject item in items)
        {
            Instantiate(item, transform.position, Quaternion.identity);
        }
    }
}
