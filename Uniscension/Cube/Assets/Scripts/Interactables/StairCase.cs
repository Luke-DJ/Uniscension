using System.Collections;
using UnityEngine;

[ExecuteInEditMode]
public class StairCase : MonoBehaviour
{
    public bool reversed = false;
    public int numSteps = 10;
    public float stepHeightOffset = 0.2f;
    public float stepWidthOffset = 0.4f;
    public GameObject stepPrefab;
    public GameObject lastStepPrefab;
    public Material stepMaterial;
    public bool autoClose = false;
    public float autoCloseTime = 0;
    public bool moveX = false;
    public bool moveY = false;
    public bool moveZ = false;
    public float moveSpeed = 1;
    public float moveDistance = 1;
    public bool startClosed = false;

    private ArrayList steps;
    private int lastNumSteps = 0;
    private bool lastReversed = false;
    private float moving;

    private void Awake()
    {
        steps = new ArrayList();
        RefreshSteps();
    }

    private void Start()
    {
        moving = Time.time;
	}

    private float closed = 0;

	private void Update()
    {
        if (steps == null)
        {
            steps = new ArrayList();
        }

		if (lastNumSteps != numSteps || lastReversed != reversed)
        {
            lastReversed = reversed;
            lastNumSteps = numSteps;

            RefreshSteps();
            PlaceSteps();

            if (startClosed)
            {
                StartCoroutine(CloseStepsWait(1));
            }
        }        
    }

    public void PlaceSteps()
    {
        for (int i = 0; i < steps.Count; i++)
        {
            GameObject step = steps[i] as GameObject;
            if (step != null)
            {
                Vector3 worldPos = new Vector3(0, stepHeightOffset * i, stepWidthOffset * i);

                if (reversed)
                {
                    worldPos = -worldPos;
                }

                Vector3 scale = transform.localScale;

                step.transform.position = transform.TransformPoint(new Vector3(worldPos.x, worldPos.y, worldPos.z));

                Slider slider = step.GetComponent<Slider>();
                if (slider != null)
                {
                    slider.Initialize(!startClosed);
                }
            }
        }
    }

    public void RefreshSteps()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);

            DestroyImmediate(child.gameObject);
        }

        foreach (GameObject step in steps)
        {
            if (step != null)
            {
                DestroyImmediate(step);
            }
        }

        steps.Clear();

        if (stepPrefab == null) { return; }

        for (int i = 0; i < numSteps; i++)
        {
            GameObject prefab = stepPrefab;

            if (lastStepPrefab != null)
            {
                if (!reversed && i == 0)
                {
                    prefab = lastStepPrefab;
                }
                else if (reversed && i == numSteps - 1)
                {
                    prefab = lastStepPrefab;
                }
            }

            GameObject step = Instantiate(prefab);

            if (step != null)
            {
                Slider slider = step.GetComponent<Slider>();
                if (slider != null)
                {
                    slider.moveX = moveX;
                    slider.moveY = moveY;
                    slider.moveZ = moveZ;
                    slider.speed = moveSpeed;
                    slider.distance = moveDistance;

                    if (startClosed)
                    {
                        slider.SnapOpen();
                    }
                }

                steps.Add(step);
                step.transform.parent = transform;
                step.transform.rotation = transform.rotation;
                step.transform.localScale = Vector3.one;

                if (stepMaterial != null)
                {
                    MeshRenderer mr = step.GetComponent<MeshRenderer>();
                    if (mr != null)
                    {
                        mr.material = stepMaterial;
                    }
                }
            }
        }
    }

    public void OpenSteps(float delay = 0)
    {
        if (moving > Time.time)
        {
            Debug.Log("return");
            return;
        }
        moving = Time.time + 10000;

        Debug.Log("open steps");

        StartCoroutine(OpenStepsRoutine(delay));
    }

    public void CloseSteps(float delay = 0)
    {
        if (moving > Time.time)
        {
            return;
        }
        moving = Time.time + 10000;

        StartCoroutine(CloseStepsRoutine(delay));
    }

    private IEnumerator CloseStepsWait(float time)
    {
        yield return new WaitForSeconds(time);

        StartCoroutine(CloseStepsRoutine(0));
    }
    private IEnumerator OpenStepsRoutine(float delay = 0)
    {
        for (int i = 0; i < steps.Count; i++)
        {
            if (delay > 0)
            {
                yield return new WaitForSeconds(delay);
            }

            GameObject step = steps[i] as GameObject;
            if (step != null)
            {
                Slider train = step.GetComponent<Slider>();
                if (train != null)
                {
                    train.OpenSlider();

                    Debug.Log("open train");

                    if (autoClose && autoCloseTime != 0)
                    {
                        StartCoroutine(CloseStepsRoutine(delay, autoCloseTime));
                    }
                }
            }
        }

        moving = Time.time + moveSpeed;

        yield return null;
    }

    private IEnumerator CloseStepsRoutine(float delay = 0, float wait = 0)
    {
        if (wait != 0)
        {
            yield return new WaitForSeconds(wait);
        }

        for (int i = 0; i < steps.Count; i++)
        {
            if (delay > 0)
            {
                yield return new WaitForSeconds(delay);
            }

            GameObject step = steps[i] as GameObject;
            if (step != null)
            {
                Slider train = step.GetComponent<Slider>();
                if (train != null)
                {
                    train.CloseSlider();
                }
            }
        }

        moving = Time.time + moveSpeed;

        yield return null;
    }
}
