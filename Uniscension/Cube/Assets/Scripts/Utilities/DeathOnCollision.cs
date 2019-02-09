using UnityEngine;

public class DeathOnCollision : MonoBehaviour
{
    [SerializeField] [Tooltip("The tag used by the players")] private string playerTagName;
    [SerializeField] [Tooltip("")] private GameObject gameOverPanel;
    [SerializeField] [Tooltip("")] private AudioSource gameOverAudio;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == playerTagName)
        {
            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(true);
            }
            
            AudioManager._instance.StopSounds();
            gameOverAudio.Play();
            Time.timeScale = 0.0f;

            if ((int)GameManager._instance.currentScore > PlayerPrefs.GetInt("highScore"))
            {
                PlayerPrefs.SetInt("highScore", (int)GameManager._instance.currentScore);
                PlayerPrefs.Save();
            }
        }
    }
}