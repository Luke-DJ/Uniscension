using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    // FPS counter variables
    [HideInInspector] public bool fpsCounter;
    [SerializeField] [Tooltip("")] private TextAnchor textAnchor;
    [SerializeField] [Tooltip("")] private int textSizeModifier;
    [SerializeField] [Tooltip("The colour of the FPS text")] private Color textColour;
    private float deltaTime;
    private GUIStyle style;
    private int textSizeModifierLimit = 100;

    public static UIManager _instance;

    private void OnValidate()
    {
        if (textSizeModifier < 1)
        {
            textSizeModifier = 1;
        }
        else if (textSizeModifier > textSizeModifierLimit)
        {
            textSizeModifier = textSizeModifierLimit;
        }
    }

    private void Awake()
    {
        if(_instance == null)
        {
            DontDestroyOnLoad(gameObject);
            _instance = this;
        }
        else
        {
            DestroyImmediate(gameObject);
        }

        fpsCounter = Convert.ToBoolean(PlayerPrefs.GetInt("fpsCounter"));
    }

    private void Start()
    {
        AudioManager._instance.SetMusicType(AudioManager.MusicType.MENU);

        // FPS counter variable initialisation
        deltaTime = 0.0f;
        style = new GUIStyle();
    }

    private void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    // For the FPS counter
    private void OnGUI()
    {
        if (fpsCounter)
        {
            int width = Screen.width;
            int height = Screen.height;

            Rect rect = new Rect(0, 0, width, height * 2 / 100);

            style.alignment = textAnchor;
            style.fontSize = (height * 2) / Mathf.Max((textSizeModifierLimit - textSizeModifier), 1);
            style.normal.textColor = textColour;

            float fps = 1.0f / deltaTime;
            string text = (int)fps + " fps";

            GUI.Label(rect, text, style);
        }
    }

    public void ChangeScene(string sceneName)
    {
        AudioManager._instance.SetMusicType(AudioManager.MusicType.PLAY);
        SceneManager.LoadScene(sceneName);
    }

    public void ToggleFPS(bool Boolean)
    {
        fpsCounter = Boolean;
        PlayerPrefs.SetInt("fpsCounter", Convert.ToInt32(Boolean));
        PlayerPrefs.Save();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}