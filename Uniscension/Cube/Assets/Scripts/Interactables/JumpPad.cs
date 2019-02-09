using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour {

    public float force = 10f;
    public float distance = 5f;
    public float effectRange = 2f;
    [Tooltip("If set to true, the jump pad will only apply force when touched")]
    public bool bounce = false;
    public bool bouncePlusHitSpeed = true;
    [Header("Spring")]
    public float springHeight = 1;
    public bool triggerInfront = false;
    public float triggerInfrontDistance = 1f;

    private GameObject lastCollided;
    private GameObject padObject;
    private GameObject springObject;
    private float springExtension = 0;
    private bool bouncing = false;
    private float originalPadPos;
    private Vector3 originalSpringScale;
    private AudioSource jumpPadSoundSource;

	// Use this for initialization
	void Start () {
        jumpPadSoundSource = gameObject.GetComponent<AudioSource>();

        Transform padTransform = transform.Find("Jump-pad_pad");
        if(padTransform != null)
        {
            padObject = padTransform.gameObject;

            originalPadPos = padObject.transform.localPosition.y;
        }

        Transform springTransform = transform.Find("Jump-pad_Spring");
        if(springTransform != null)
        {
            springObject = springTransform.gameObject;

            originalSpringScale = springTransform.localScale;
        }
	}
	
	// Update is called once per frame
	void Update () {
        float range = effectRange * 0.5f;

        if (GameManager._instance != null)
        {
            foreach (GameObject playerObject in GameManager._instance.players)
            {
                if (!bounce)
                {
                    Vector3 playerPos = playerObject.transform.position;
                    Vector3 pos = transform.position;

                    Vector3 playerLocalPos = transform.InverseTransformPoint(playerPos);

                    if (playerLocalPos.x > -range && playerLocalPos.x < range && playerLocalPos.z > -range && playerLocalPos.z < range)
                    {
                        ApplyForce(playerObject);
                    }
                }

                if (triggerInfront)
                {
                    Vector3 normal = (playerObject.transform.position - transform.position).normalized;
                    float dot = Vector3.Dot(transform.forward, normal);

                    if (dot > 0.45f)
                    {
                        float distance = Vector3.Distance(transform.position, playerObject.transform.position);
                        if (distance < triggerInfrontDistance)
                        {
                            ApplyForce(playerObject);
                        }
                    }
                }
            }
        }

        if(padObject != null && springObject != null)
        {
            float padDiff = 1 - springExtension;

            if (bouncing)
            {                
                springExtension = Mathf.Min(1, springExtension + (padDiff * 0.2f));

                if (springExtension >= 0.99f)
                {
                    springExtension = 1;
                    bouncing = false;
                }
                ApplySpringExtension(springExtension);
            }
            else if(springExtension > 0)
            {
                springExtension = Mathf.Min(1, springExtension * 0.9f);

                if (springExtension <= 0.01f)
                {
                    springExtension = 0;
                }
                ApplySpringExtension(springExtension);
            }
        }
    }

    private void ApplySpringExtension(float extension)
    {
        Vector3 pos = transform.position;

        if(padObject != null)
        {
            float mult = (springExtension * springHeight);
            Vector3 forward = transform.up;
            padObject.transform.position = new Vector3(pos.x + (forward.x * mult), pos.y + (mult * forward.y), pos.z + (mult * forward.z));
        }

        if(springObject != null)
        {
            springObject.transform.localScale = new Vector3(originalSpringScale.x, (originalSpringScale.y * ((springExtension * 12) + 1) * springHeight), originalSpringScale.z);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        GameObject hitObject = collision.gameObject;

        ApplyForce(hitObject, collision.relativeVelocity.magnitude);
    }

    private void ApplyForce(GameObject playerObject, float relativeVelocity = 0)
    {
        float yDiff = playerObject.transform.position.y - transform.position.y;
        if(yDiff > 0 && yDiff < distance)
        {
            Rigidbody rb = playerObject.GetComponent<Rigidbody>();
            if(rb != null)
            {
                if (bounce)
                {
                    float velocitySet = force * 0.02f;

                    velocitySet = Mathf.Max(velocitySet, bouncePlusHitSpeed ? relativeVelocity : 0);

                    
                    rb.velocity = transform.up * velocitySet;
                    playerObject.transform.position += transform.up;
                }
                else
                {
                    rb.AddForce(transform.up * force);
                }

                bouncing = true;

                if(jumpPadSoundSource != null)
                {
                    jumpPadSoundSource.Stop();
                    jumpPadSoundSource.pitch = Random.Range(0.8f, 1.2f);
                    jumpPadSoundSource.Play();
                }
            }
        }
    }
}
