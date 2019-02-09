using UnityEngine;

public class MenuGameManager : MonoBehaviour
{
	private void Start()
    {
        AudioManager._instance.StopSounds();
        AudioManager._instance.PlaySound(AudioManager.MusicType.MENU);
        Time.timeScale = 1;
	}
}