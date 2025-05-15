using UnityEngine;

public abstract class AudioSO : ScriptableObject
{
    [field: SerializeField]
    public AudioClip AudioClip { get; private set; }

    [field: Tooltip("Value is [0.01, 1], where 0 means no sound and 1 means no change. Values in between will lower the volume.")]
    [field: SerializeField]
    [field: Range(0.01f, 1)]
    public float AudioBalancingValue { get; private set; } = 1f;
}
