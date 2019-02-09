using UnityEngine;

public class HighScoreText : MonoBehaviour
{
    private TextMesh textMesh;

	private void Start()
    {
        textMesh = GetComponent<TextMesh>();
    }

	private void Update()
    {
		if (textMesh != null)
        {
            textMesh.text = "HIGH SCORE: " + PlayerPrefs.GetInt("highScore");
        }
	}
}
