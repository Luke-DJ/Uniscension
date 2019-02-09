using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayUIManager : MonoBehaviour
{
    [SerializeField] [Tooltip("")] private Text scoreText;
    [SerializeField] [Tooltip("")] private GameObject pausePanel;
    [SerializeField] [Tooltip("")] private GameObject settingsPanel;
    [SerializeField] [Tooltip("")] private GameObject gameOverPanel;

    private void Update()
    {
        int score = (int)GameManager._instance.currentScore;
        if (score > 0)
        {
            scoreText.text = score.ToString();
        }
        else
        {
            scoreText.text = "";
        }
        

        if (Input.GetButtonDown("Pause"))
        {
            if (!gameOverPanel.activeSelf)
            {
                if (settingsPanel.activeSelf)
                {
                    pausePanel.SetActive(true);
                    settingsPanel.SetActive(false);
                }
                else
                {
                    Time.timeScale = Convert.ToInt32(pausePanel.activeSelf);
                    pausePanel.SetActive(!pausePanel.activeSelf);
                }
            }
        }
    }

    public void ChangeScene(string sceneName)
    {
        UIManager._instance.ChangeScene(sceneName);
    }

    public void ToggleFPS(bool Boolean)
    {
        UIManager._instance.ToggleFPS(Boolean);
    }

    public void SetTimeScale(float timeScale)
    {
        Time.timeScale = timeScale;
    }
}