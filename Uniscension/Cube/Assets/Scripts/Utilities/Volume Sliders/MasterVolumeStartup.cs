using UnityEngine;
using UnityEngine.UI;

public class MasterVolumeStartup : MonoBehaviour
{
	private void Start()
    {
        float volumeLevel;
        if (AudioManager._instance.audioMixer.GetFloat(AudioManager._instance.masterVolumeExposedParameterName, out volumeLevel))
        {
            gameObject.GetComponent<UnityEngine.UI.Slider>().value = volumeLevel;
        }
	}
}