using UnityEngine;

[CreateAssetMenu(fileName = "NewMusicSO", menuName = "AudioLogic/MusicSO")]
public class MusicSO : AudioSO
{
    [field: SerializeField]
    public MusicName MusicName { get; private set; }
}
