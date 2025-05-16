using UnityEngine;

[CreateAssetMenu(fileName = "ObjectPlacementData", menuName = "PCG/ObjectPlacement")]
public class ObjectPlacementSO : ScriptableObject
{
    [field: SerializeField]
    public int SpawnChance { get; private set; }
    [field: SerializeField]
    public int SpawnAmount { get; private set; }
    [field: SerializeField]
    public int MaxSpawnAmount { get; private set; }
    [field: SerializeField]
    public GameObject Prefab { get; private set; }
    [field: SerializeField]
    public bool HasSpawnIteration { get; private set; }
    [field: SerializeField]
    public int SpawnIteration { get; private set; }
}