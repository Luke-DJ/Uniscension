using UnityEngine;

public class MoveToPos : MonoBehaviour
{
    public float movement = 0.1f;
    public float delay = 0;
    public Vector3 targetOffset;
    public bool relative = false;
    public bool dontSmooth = false;

    private float startTime;
    private Vector3 originalPos;

	private void Start()
    {
        startTime = Time.time + delay;
        originalPos = transform.position;
    }

	private void Update()
    {
        if (delay > 0)
        {
            if(Time.time < startTime) { return; }
        }

        if (dontSmooth)
        {
            Vector3 relative = (targetOffset + originalPos) - transform.position;
            transform.position += relative.normalized * movement * Time.deltaTime;
        }
        else
        {
            Vector3 pos = transform.position;
            Vector3 target;
            if (relative)
            {
                target = originalPos + targetOffset;
            }
            else
            {
                target = targetOffset;
            }
            Vector3 diff = target - pos;
            float distance = diff.magnitude;

            transform.position = pos + (diff * distance * movement * Time.deltaTime);
        }
	}
}
