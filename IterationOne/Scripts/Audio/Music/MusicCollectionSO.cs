using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMusicCollectionSO", menuName = "AudioLogic/MusicCollectionSO")]
public class MusicCollectionSO : ScriptableObject
{
    [SerializeField]
    private List<MusicSO> musicSongs;

    public MusicSO FindSong(MusicName musicName)
    {
        MusicSO selectedSong = musicSongs.Find(song => song.MusicName == musicName);
        if (selectedSong == null)
        {
            Debug.LogError($"Could not find song with name: {musicName}");
            return null;
        }
        return selectedSong;
    }
}
