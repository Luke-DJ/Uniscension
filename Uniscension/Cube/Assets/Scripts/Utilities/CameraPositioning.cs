using UnityEngine;

public class CameraPositioning : MonoBehaviour
{
    /*
    [SerializeField] [Tooltip("The target that the camera will rotate around")] private GameObject target;
    [SerializeField] [Tooltip("The speed at which the camera will rotate around the target")] public float speed;
    [SerializeField] [Tooltip("An offset that will be applied to the camera's position, relative to the position of the target")] private Vector3 offset;
    */

    public float followSpeed = 0.1f;
    public float originX = 0;
    public float originZ = 0;
    public float baseZoom = 30;
    public float orthographicScale = 1;
    public float yOffset = 10f;
    public bool rotateWithTarget = false;

    public GameObject[] targets;

    private Camera camera;
    private float rotation = 0;
    private Vector3 targetPosition;
    private float targetOffsetY;
    private float currentOffsetY;
    private float currentLookY;

    private void Start()
    {
        // Calculating the offset 
        //offset = new Vector3(target.transform.position.x + offset.x, target.transform.position.y + offset.y, target.transform.position.z + offset.z);

        camera = gameObject.GetComponent<Camera>();
        targetPosition = transform.position;
        currentOffsetY = transform.position.y;
    }

    // Unity Documentation specifies that: "a follow camera should always be implemented in LateUpdate because it tracks objects that might have moved inside Update"
    private void LateUpdate()
    {
        // Quaternion.AngleAxis = Creates a rotation which rotates angle degrees around axis
        // Vector3.up = Shorthand for writing Vector3(0, 1, 0)
        // Rotation will therefore happen around the Y axis

        /*
        offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * speed, Vector3.up) * offset;
        transform.position = target.transform.position + offset;
        transform.LookAt(target.transform.position);
        */

        float biggestDistance = 0;
        float totalY = 0;

        foreach(GameObject target in targets)
        {
            if(target == null) { continue; }

            totalY += target.transform.position.y;
            
            foreach(GameObject target2 in targets)
            {
                if(target2 == null) { continue; }

                float distance = Mathf.Abs(target.transform.position.y - target2.transform.position.y);
                biggestDistance = Mathf.Max(biggestDistance, distance);
            }
        }

        biggestDistance = Mathf.Max(0, biggestDistance - (baseZoom * 0.3f));
        float zoom = biggestDistance + baseZoom;

        float averageY = totalY / targets.Length;

        float orbitX, orbitZ;

        if (rotateWithTarget)
        {
            Vector3 origin = new Vector3(originX, averageY, originZ);
            Vector3 relativePosition = (new Vector3(targets[0].transform.position.x, averageY, targets[0].transform.position.z) - origin).normalized;
            
            orbitX = (origin + (relativePosition * zoom)).x;
            orbitZ = (origin + (relativePosition * zoom)).z;
        }
        else
        {
            orbitX = Mathf.Cos(rotation) * zoom;
            orbitZ = Mathf.Sin(rotation) * zoom;
        }
        

        targetOffsetY = averageY + yOffset;

        currentOffsetY = Mathf.MoveTowards(currentOffsetY, targetOffsetY, Mathf.Abs(targetOffsetY - currentOffsetY) * 0.04f);

        targetPosition = new Vector3(originX + orbitX, currentOffsetY, originZ + orbitZ);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Vector3.Distance(transform.position, targetPosition) * followSpeed);

        if(camera != null)
        {
            if (camera.orthographic)
            {
                camera.orthographicSize = zoom * 0.7f * orthographicScale;
            }
        }

        if(currentLookY == 0)
        {
            currentLookY = averageY;
        }
        else
        {
            currentLookY = Mathf.MoveTowards(currentLookY, averageY, Mathf.Abs(averageY - currentLookY) * 0.1f);
        }

        transform.LookAt(new Vector3(originX, currentLookY, originZ));
        rotation -= Input.GetAxis("Mouse X") * 0.1f;

        float cameraMove = 0.1f;
        float time = Time.time * 0.25f;

        transform.position = new Vector3(transform.position.x + (Mathf.Cos(time) * cameraMove), transform.position.y + (Mathf.Sin(time) * cameraMove), transform.position.z + (Mathf.Cos(time) * cameraMove) + (Mathf.Sin(time) * cameraMove));
        /*
        offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * speed, Vector3.up) * offset;
        transform.position = target.transform.position + offset;
        transform.LookAt(averagePosition);
        */
    }
}