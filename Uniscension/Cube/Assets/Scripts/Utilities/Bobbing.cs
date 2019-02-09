using System;
using UnityEngine;

public class Bobbing : MonoBehaviour
{
    public bool originalPos = true;
    [SerializeField] [Tooltip("The speed of the bobbing")] private float bobbingSpeed;
    [SerializeField] [Tooltip("The Y increase and decrease of the bobbing")] private float bobbingStrength;
    [SerializeField] [Tooltip("How many seconds to offset the bobbing by")] private float timeDelay;
    private float originalY;
    private float lastY;

    private void OnValidate()
    {
        if (bobbingSpeed < 0)
        {
            bobbingSpeed = 0;
        }
        if (bobbingStrength < 0)
        {
            bobbingStrength = 0;
        }
        if (timeDelay < 0)
        {
            timeDelay = 0;
        }
    }

    private void Start()
    {
        originalY = gameObject.transform.position.y;
        lastY = originalY;
    }

    private void FixedUpdate()
    {
        float diffY = transform.position.y - lastY;

        if (diffY != 0)
        {
            originalY += diffY;
        }

        if (originalPos)
        {
            gameObject.transform.position = new Vector3(transform.position.x, originalY + (float)(Math.Sin((Time.fixedTime + timeDelay) * bobbingSpeed) * bobbingStrength), transform.position.z);
        }
        else
        {
            gameObject.transform.position = new Vector3(transform.position.x, transform.position.y + (float)(Math.Sin((Time.fixedTime + timeDelay) * bobbingSpeed) * bobbingStrength), transform.position.z);
        }

        lastY = gameObject.transform.position.y;
    }
}