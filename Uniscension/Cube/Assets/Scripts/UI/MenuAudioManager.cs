using UnityEngine;

public class MenuAudioManager : MonoBehaviour
{
    public void SetMasterGroupVolume(float volumeLevel)
    {
        AudioManager._instance.SetMasterGroupVolume(volumeLevel);
    }
    public void SetMusicGroupVolume(float volumeLevel)
    {
        AudioManager._instance.SetMusicGroupVolume(volumeLevel);
    }

    public void SetSFXGroupVolume(float volumeLevel)
    {
        AudioManager._instance.SetSFXGroupVolume(volumeLevel);
    }
}