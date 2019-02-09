using UnityEngine;
using UnityEngine.UI;

public class MusicVolumeStartup : MonoBehaviour
{
    private void Start()
    {
        float volumeLevel;
        if (AudioManager._instance.audioMixer.GetFloat(AudioManager._instance.musicVolumeExposedParameterName, out volumeLevel))
        {
            gameObject.GetComponent<UnityEngine.UI.Slider>().value = volumeLevel;
        }
    }
}