using UnityEngine;

[CreateAssetMenu(fileName = "FOVSettings", menuName = "FOV/FOV Settings")]
public class FOVSettings : ScriptableObject
{
    [field: SerializeField]
    [field: Min(0.1f)]
    public float Radius { get; private set; } = 5f;

    [field: SerializeField]
    [field: Range(1, 360)]
    public float Angle { get; private set; } = 90f;

    [field: SerializeField]
    public LayerMask TargetMask { get; private set; }

    [field: SerializeField]
    public LayerMask BlockingMask { get; private set; }

    [field: SerializeField]
    [Tooltip("How often the FOV coroutine executes. Can't be changed during runtime.")]
    public float SearchFrequency { get; private set; } = 0.2f;

    [field: SerializeField]
    public bool DisplayGizmos { get; private set; } = true;
    public Color RadiusColor { get; private set; } = Color.white;
    public Color AnglesColor { get; private set; } = Color.yellow;
    public Color InSightColor { get; private set; } = Color.green;
}
