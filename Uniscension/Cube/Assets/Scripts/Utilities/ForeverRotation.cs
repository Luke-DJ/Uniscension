using UnityEngine;

public class ForeverRotation : MonoBehaviour
{
    [SerializeField] [Tooltip("")] private float speed;

    private void FixedUpdate()
    {
        gameObject.transform.Rotate(Vector3.up * ((speed * 360) * Time.fixedDeltaTime));
    }
}