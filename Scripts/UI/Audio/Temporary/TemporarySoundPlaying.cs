using UnityEngine;

public class TemporarySoundPlaying : MonoBehaviour
{
    [SerializeField]
    private SoundPlayer soundPlayer;

    // Not possible to attach PlaySound in OnClick in Inspector directly 
    // since the parameter is an enum, this is one possible solution.
    // In case we want real buttons to play sounds then they would
    // probably have their own sound player and script similar to this.
    public void PlayExampleSound()
    {
        soundPlayer.PlaySound(SoundName.Example1);
    }
}
