using UnityEngine;

public class MultiTrigger : MonoBehaviour
{
    public enum TriggerType { TANKTRAIN_START_MOVE, TANKTRAIN_END_MOVE }

    public TriggerType triggerType;
    public float triggerDelay = 0;
    public GameObject[] objects;

    private int triggerIndex = 0;
    private float lastTrigger = 0f;
    private bool triggering = false;

	private void Update()
    {
        if (triggering && objects != null && objects.Length > 0)
        {
            if (Time.time - lastTrigger < triggerDelay) { return; }
            lastTrigger = Time.time;

            if (triggerDelay == 0)
            {
                for (int i = 0; i < objects.Length; i++)
                {
                    FireOutput(objects[i]);
                }

                triggerIndex = objects.Length - 1;
            }
            else
            {
                int i = triggerIndex;
                FireOutput(objects[i]);

                triggerIndex++;
            }

            if (triggerIndex >= objects.Length)
            {
                triggering = false;
            }
            
        }
        
    }

    public void FireOutputs()
    {
        triggerIndex = 0;
        lastTrigger = 0f;
        triggering = true;
    }

    private void FireOutput(GameObject triggerObject)
    {
        TankTrain train;

        if (triggerObject != null)
        {
            switch (triggerType)
            {
                case TriggerType.TANKTRAIN_END_MOVE:
                    train = triggerObject.GetComponent<TankTrain>();
                    if (train != null)
                    {
                        train.StopMoving();
                    }
                    break;
                case TriggerType.TANKTRAIN_START_MOVE:
                    train = triggerObject.GetComponent<TankTrain>();
                    if (train != null)
                    {
                        train.StartMoving();
                    }
                    break;
            }
        }
    }
}
