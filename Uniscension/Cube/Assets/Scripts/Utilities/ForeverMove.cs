using UnityEngine;

public class ForeverMove : MonoBehaviour
{
    [SerializeField] [Tooltip("Offsets the GameObject's position by these values every frame")] private Vector3 offset;
    private Vector3 originalPosition;

    private void FixedUpdate()
    {
        originalPosition = gameObject.transform.position;
        gameObject.transform.position = new Vector3(originalPosition.x + offset.x, originalPosition.y + offset.y, originalPosition.z + offset.z);
	}
}