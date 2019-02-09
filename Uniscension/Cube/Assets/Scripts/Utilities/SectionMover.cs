using System.Collections;
using UnityEngine;

public class SectionMover : MonoBehaviour
{
    [SerializeField] [Tooltip("Decreases the y position of sections by this amount every second")] private float yDecrease;
    [SerializeField] [Tooltip("How many seconds before the sections start moving")] private float initialDelay;
    [SerializeField] [Tooltip("The change to the value of yDecrease every second")] private float increaseOverTime;
    [HideInInspector] public ArrayList sections;
    private bool delayActive;

    private void OnValidate()
    {
        if (yDecrease < 0)
        {
            yDecrease = 0;
        }
        if (initialDelay < 0)
        {
            initialDelay = 0;
        }
        if (increaseOverTime < 0)
        {
            increaseOverTime = 0;
        }
    }

    private void Start()
    {
        if (initialDelay > 0)
        {
            delayActive = true;
            StartCoroutine(Delay());
        }
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(initialDelay);
        delayActive = false;
    }

    private void FixedUpdate()
    {
        if (sections == null)
        {
            return;
        }

        if (!delayActive)
        {
            /*foreach (GameObject player in GameManager._instance.players)
            {
                DecreasePosition(player);
            }*/

            foreach (GameObject section in sections)
            {
                DecreasePosition(section);
            }

            yDecrease += increaseOverTime * Time.fixedDeltaTime;

            GameManager._instance.currentScore += yDecrease * 0.1f; ;
        }
	}

    private void DecreasePosition(GameObject targetObject)
    {
        Vector3 position = targetObject.transform.position;
        position.y -= yDecrease * Time.fixedDeltaTime;
        targetObject.transform.position = position;
    }
}