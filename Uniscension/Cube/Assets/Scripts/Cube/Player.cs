using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 10;
    public float jumpForce = 10;
    public float maxJump = 1;

    [Header("Input")]
    public string[] horizontalInput;
    public string[] jumpInput;
    public string[] useInput;

    [Header("Audio")]
    public AudioSource jumpSource;

    public int playerIndex;
    [HideInInspector]
    public float lastVelocity = 0;

    public Cube cube;
    [SerializeField]
    private float cubeDistance = 1;

    private bool jumping = false;
    private float jumpHeight = 0f;
    private float lastJumpHeight = 0f;

    private Quaternion targetRotation;
    private Cube.CubeSide cubeSide;
    private Rigidbody rb;
    private Transform mouthTransform;

    // Use this for initialization
    void Start()
    {

        targetRotation = transform.rotation;
        rb = GetComponent<Rigidbody>();

        if (cube != null)
        {
            CorrectCubeSide();
            CorrectPosition();
            CorrectAngle(180);
        }

        mouthTransform = transform.Find("Mouth");

        if(GameManager._instance != null)
        {
            GameManager._instance.SetupPlayer(gameObject, playerIndex);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(mouthTransform != null)
        {
            Vector3 scale = mouthTransform.transform.localScale;
            mouthTransform.transform.localScale = new Vector3(scale.x, scale.y, Mathf.Cos(Time.time * 8) * 0.1f);
        }

        if (cube != null)
        {
            CorrectPosition();

            CorrectAngle(3);

            float movement = GetRightValue();

            if (movement > 0)
            {
                MoveRight(movement);
            }
            else if (movement < 0)
            {
                MoveLeft(-movement);
            }

            PlayerButtonPressed jumpInput = GetJumpPressed();

            if (jumpInput.pressed)
            {
                Jump();
            }
        }

        if (IsGrounded())
        {
            if (jumping)
            {
                jumping = false;
                CorrectAngle(180);
            }

            RaycastHit hit = GetGroundedHit();
            if(hit.collider != null)
            {
                Vector3 normal = new Vector3(hit.normal.x, 0, hit.normal.z);

                CorrectAngle(180, normal);
            }
        }
        else
        {
            float height = transform.position.y;

            if (jumping)
            {
                if (height > lastJumpHeight)
                {
                    float diffHeight = (jumpHeight + maxJump) - height;
                    rb.velocity = new Vector3(rb.velocity.x, diffHeight * 0.2f / Time.deltaTime, rb.velocity.z);

                    if (diffHeight < 0.1f || rb.velocity.y <= 0.1f)
                    {
                        jumping = false;
                    }
                }
            }
            else
            {
                if(GroundedTo(0.8f) == null)
                {
                    rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y - (40 * Time.deltaTime), rb.velocity.z);
                }
            }

            lastJumpHeight = height;
        }
    }

    public float GetRightValue()
    {
        string axis = GetRightAxis();

        if(axis == "") { return 0; }

        return Input.GetAxis(axis);
    }

    public string GetRightAxis()
    {
        if (horizontalInput.Length <= playerIndex) { return ""; }

        return horizontalInput[playerIndex];
    }

    public PlayerButtonPressed GetJumpPressed()
    {
        string button = GetJumpKey();

        PlayerButtonPressed pressed = new PlayerButtonPressed();
        pressed.held = false;
        pressed.pressed = false;

        if (button.Equals("")) { return pressed; }

        pressed.held = Input.GetButton(button);
        pressed.pressed = Input.GetButtonDown(button);

        return pressed;
    }

    public string GetJumpKey()
    {
        if (jumpInput.Length <= playerIndex) { return ""; }

        return jumpInput[playerIndex];
    }

    public PlayerButtonPressed GetUsePressed()
    {
        string button = GetUseKey();

        PlayerButtonPressed pressed = new PlayerButtonPressed();
        pressed.held = false;
        pressed.pressed = false;

        if (button.Equals("")) { return pressed; }

        pressed.held = Input.GetButton(button);
        pressed.pressed = Input.GetButtonDown(button);

        return pressed;
    }

    public string GetUseKey()
    {
        if (useInput.Length <= playerIndex) { return ""; }

        return useInput[playerIndex];
    }

    public void CorrectCubeSide()
    {
        cubeSide = cube.GetCubeSide(transform.position);
    }

    public void CorrectAngle(float degree, Vector3 normal)
    {
        Vector3 up = cube.GetCubeSideGravity(cubeSide);

        targetRotation = Quaternion.LookRotation(-cube.transform.up, up);

        if(normal != Vector3.zero)
        {
            Quaternion rotate;

            if (cubeSide == Cube.CubeSide.LEFT)
            {
                rotate = Quaternion.Euler(normal.x * 60, normal.z * 60, 0);
            }
            else if (cubeSide == Cube.CubeSide.RIGHT)
            {
                rotate = Quaternion.Euler(normal.x * 60, -normal.z * 60, 0);
            }
            else
            {
                rotate = Quaternion.Euler(normal.z * 60, -normal.x * 60, 0);
            }
            
            targetRotation *= rotate;
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, degree);
    }

    public void CorrectAngle(float degree)
    {
        CorrectAngle(degree, Vector3.zero);
    }

    public void CorrectPosition()
    {
        Vector3 cubeSideOrigin = cube.GetCubeSideOrigin(cubeSide, cubeDistance);
        Vector3 up = cube.GetCubeSideGravity(cubeSide);

        Vector3 pos = transform.position;

        float scaleLimit = (cube.scale * 0.5f) + cubeDistance;

        if (cubeSide == Cube.CubeSide.RIGHT || cubeSide == Cube.CubeSide.LEFT)
        {
            float diffZ = pos.z - cubeSideOrigin.z;

            transform.position = new Vector3(cubeSideOrigin.x, pos.y, diffZ);

            if (diffZ * -up.x > scaleLimit)
            {
                cubeSide = cube.GetNextCubeSide(cubeSide, true);
            }
            else if (diffZ * -up.x < -scaleLimit)
            {
                cubeSide = cube.GetNextCubeSide(cubeSide, false);
            }
        }
        else if (cubeSide == Cube.CubeSide.FRONT || cubeSide == Cube.CubeSide.BACK)
        {
            float diffX = pos.x - cubeSideOrigin.x;

            transform.position = new Vector3(diffX, pos.y, cubeSideOrigin.z);

            if (diffX * -up.z > scaleLimit)
            {
                cubeSide = cube.GetNextCubeSide(cubeSide, false);
            }
            else if (diffX * -up.z < -scaleLimit)
            {
                cubeSide = cube.GetNextCubeSide(cubeSide, true);
            }
        }
    }

    public void MoveRight(float perc = 1)
    {
        float movement = speed * Time.deltaTime;
        float hitRight = RaycastDirection(transform.right, perc * 0.25f);

        if (hitRight >= 0)
        {
            perc = Mathf.Min(hitRight, perc);
        }

        perc = perc + 0.1f;

        if (perc > 0)
        {
            transform.position += transform.right * movement * perc;
        }
    }

    public void MoveLeft(float perc = 1)
    {
        float movement = speed * Time.deltaTime;
        float hitRight = RaycastDirection(-transform.right, perc * 0.25f);

        if (hitRight >= 0)
        {
            perc = Mathf.Min(hitRight, perc);
        }

        perc = perc + 0.1f;

        if (perc > 0)
        {
            transform.position -= transform.right * movement * perc;
        }
    }

    public void Jump()
    {
        if (!IsGrounded()) { return; }
        rb.AddForce(Vector3.up * jumpForce * rb.mass * 2);

        jumping = true;

        jumpHeight = transform.position.y;
        lastJumpHeight = jumpHeight;

        transform.position += new Vector3(0, 0.1f, 0);

        if(jumpSource != null)
        {
            jumpSource.Stop();
            jumpSource.Play();
        }
    }

    public bool IsGrounded()
    {
        GameObject groundedTo = GroundedTo();

        return groundedTo != null;
    }

    public GameObject GroundedTo(float distance = 0.2f)
    {
        RaycastHit hit1 = GetGroundedHit(distance);

        if(hit1.collider != null)
        {
            return hit1.collider.gameObject;
        }


        return null;
    }

    public RaycastHit GetGroundedHit(float distance = 0.2f)
    {
        RaycastHit hit1;

        int layerMask = ~LayerMask.GetMask("Attachments");

        if (Physics.BoxCast(transform.position, transform.localScale * 0.45f, transform.TransformDirection(Vector3.forward), out hit1, transform.rotation, distance, layerMask))
        {
            return hit1;
        }

        return hit1;
    }

    public float RaycastDirection(Vector3 direction, float distance = 1)
    {
        RaycastHit hit1 = RaycastDirectionHit(direction, distance);

        if(hit1.collider != null)
        {
            return hit1.distance;
        }

        return -1f;
    }

    public RaycastHit RaycastDirectionHit(Vector3 direction, float distance = 1)
    {
        RaycastHit hit1;

        int layerMask = ~LayerMask.GetMask("Attachments");

        if (Physics.BoxCast(transform.position, transform.localScale * 0.45f, direction, out hit1, transform.rotation, distance, layerMask))
        {
            return hit1;
        }

        return hit1;
    }
}

public class PlayerButtonPressed
{
    public bool pressed;
    public bool held;
}