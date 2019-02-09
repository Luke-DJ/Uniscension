using UnityEngine;
using UnityEngine.UI;

public class ToggleFPSStartup : MonoBehaviour
{
	private void Start()
    {
        gameObject.GetComponent<Toggle>().isOn = UIManager._instance.fpsCounter;
    }
}