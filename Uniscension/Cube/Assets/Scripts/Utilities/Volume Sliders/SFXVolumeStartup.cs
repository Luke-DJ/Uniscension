using UnityEngine;
using UnityEngine.UI;

public class SFXVolumeStartup : MonoBehaviour
{
    private void Start()
    {
        float volumeLevel;
        if (AudioManager._instance.audioMixer.GetFloat(AudioManager._instance.sfxVolumeExposedParameterName, out volumeLevel))
        {
            gameObject.GetComponent<UnityEngine.UI.Slider>().value = volumeLevel;
        }
    }
}