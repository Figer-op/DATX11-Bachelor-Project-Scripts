using UnityEngine;

[CreateAssetMenu(fileName = "NewSoundSO", menuName = "AudioLogic/SoundSO")]
public class SoundSO : AudioSO 
{
    [field: SerializeField]
    public SoundName SoundName { get; private set; }
}
