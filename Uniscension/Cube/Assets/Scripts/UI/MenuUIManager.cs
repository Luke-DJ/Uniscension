using UnityEngine;

public class MenuUIManager : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        UIManager._instance.ChangeScene(sceneName);
    }

    public void ToggleFPS(bool Boolean)
    {
        UIManager._instance.ToggleFPS(Boolean);
    }

    public void ExitGame()
    {
        UIManager._instance.ExitGame();
    }
}