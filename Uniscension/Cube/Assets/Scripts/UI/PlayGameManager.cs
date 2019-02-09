using UnityEngine;

public class PlayGameManager : MonoBehaviour
{
    private void Start()
    {
        AudioManager._instance.StopSounds();
        AudioManager._instance.PlaySound(AudioManager.MusicType.PLAY);
        Time.timeScale = 1;
        GameManager._instance.currentScore = 0;
    }
}