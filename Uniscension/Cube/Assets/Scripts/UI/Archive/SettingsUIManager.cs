using UnityEngine;

public class SettingsUIManager : MonoBehaviour
{
	public void ToggleFPSCounter()
    {
        Debug.Log("Test");
        //UIManager._instance.fpsCounter = !UIManager._instance.fpsCounter;
    }

    public void ChangeScene(string sceneName)
    {
        UIManager._instance.ChangeScene(sceneName);
    }
}