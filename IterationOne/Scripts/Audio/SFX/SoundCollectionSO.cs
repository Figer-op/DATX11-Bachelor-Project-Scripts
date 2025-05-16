using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSoundCollectionSO", menuName = "AudioLogic/SoundCollectionSO")]
public class SoundCollectionSO : ScriptableObject
{
    [SerializeField]
    private List<SoundSO> sounds;

    public SoundSO FindSound(SoundName soundName)
    {
        SoundSO selectedSound = sounds.Find(sound => sound.SoundName == soundName);
        if (selectedSound == null)
        {
            Debug.LogError($"Could not find sound with name: {soundName}");
            return null;
        }
        return selectedSound;
    }
}
