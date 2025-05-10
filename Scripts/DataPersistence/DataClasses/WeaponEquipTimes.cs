using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class WeaponEquipTimes
{
    [SerializeField]
    private List<float> weaponTimes = new();
    public List<float> WeaponTimes { get => weaponTimes; set => weaponTimes = value; }
}
