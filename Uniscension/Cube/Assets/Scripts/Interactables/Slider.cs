using UnityEngine;

[ExecuteInEditMode]
public class Slider : MonoBehaviour
{
    public float speed = 1;
    public float distance = 10;
    public bool moveX = false;
    public bool moveY = false;
    public bool moveZ = false;
    public bool smoothMovement = false;
    public float smoothFactor = 0.05f;

    public bool closed;

    private Vector3 originPos;
    private float lastY;

	private void Start()
    {
        Initialize();        
    }

    public void Initialize(bool closed = true)
    {
        this.closed = closed;

        originPos = transform.position;

        lastY = transform.position.y;
    }

	private void Update()
    {
        float diffY = transform.position.y - lastY;
        lastY = transform.position.y;

        originPos = new Vector3(originPos.x, originPos.y + diffY, originPos.z);

        Vector3 targetPos;

        if (closed)
        {
            targetPos = originPos;
        }
        else
        {
            targetPos = new Vector3(moveX ? distance : 0, moveY ? distance : 0, moveZ ? distance : 0) + originPos;
        }

        if (Application.isPlaying) {
            Vector3 difference = targetPos - transform.position;
            float distanceToTarget = Vector3.Distance(transform.position, targetPos);

            if (smoothMovement)
            {

                targetPos += difference * distanceToTarget * smoothFactor;
            }
            else
            {
                targetPos = transform.position + (difference * speed * Time.deltaTime);
            }

            transform.position = targetPos;
        }

        Debug.DrawLine(transform.position, targetPos, Color.red);
	}

    public void SnapClosed()
    {
        closed = true;
        transform.position = originPos;
    }

    public void SnapOpen()
    {
        Vector3 targetPos = new Vector3(moveX ? distance : 0, moveY ? distance : 0, moveZ ? distance : 0) + originPos;

        transform.position = targetPos;
        closed = false;
    }

    public void OpenSlider()
    {
        closed = false;
    }

    public void CloseSlider()
    {
        closed = true;
    }

    public void ToggleSlider()
    {
        if (closed)
        {
            OpenSlider();
        }
        else
        {
            CloseSlider();
        }
    }
}
