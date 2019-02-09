using UnityEngine;
using UnityEngine.Events;

/*
 * The Interaction class is a base class for all interactable entities in the game
 * Entities such as button and teleporter extend this class
 * This class contains behaviour associated with all interactables, such as useType and press / release logic
 * This class also contains callbacks which can be used by designers via the unity inspector to link output callbacks from interactable events when designing levels
*/

public class Interactable : MonoBehaviour
{
    public enum UseType { PRESS, TOGGLE, HOLD }

    [Tooltip("Defines the button use behaviour")]
    public UseType useType = UseType.PRESS;
    [Tooltip("Time in seconds which interactable will be disabled after use")]
    public float releaseCooldown = 0;
    public Material idleMaterial;
    public Material canInteractMaterial;
    public Material pressedMaterial;
    public Material disabledMaterial;

    public bool controlInputEnabled = true;
    public bool triggerOnCollide = false;

    public bool disabled = false;
    [Tooltip("Player will interact with this as soon as they are within range, without pressing inteact")]
    public bool autoPress;

    [Header("Audio")]
    public AudioSource useSoundSource;

    [System.Serializable]
    public class InteractableEvent : UnityEvent<GameObject> { }

    [Header("Passes GameObject of the player into the callback parameters")]
    public InteractableEvent onPressed;
    [Header("Passes GameObject of the player into the callback parameters")]
    public InteractableEvent onReleased;
    public UnityEvent onInteractableEnabled;
    public UnityEvent onInteractableDisabled;

    protected MeshRenderer mr;
    protected Material currentMeshMaterial;
    private bool pressed = false;
    private bool wasPressed = false;
    private GameObject heldBy = null;
    private float enableAt = 0f;

    // Use this for initialization
    protected virtual void Start()
    {
        mr = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (enableAt > 0 && Time.time > enableAt)
        {
            enableAt = 0f;
            disabled = false;
        }

        bool canInteract = false;

        if (disabled)
        {
            if (disabledMaterial != null)
            {
                SetMeshMaterial(disabledMaterial);
            }
        }
        else if (GameManager._instance != null)
        {
            foreach (GameObject player in GameManager._instance.players)
            {
                if (CanInteract(player) && controlInputEnabled)
                {
                    if (canInteractMaterial != null)
                    {
                        SetMeshMaterial(canInteractMaterial);
                    }
                    else if (idleMaterial != null)
                    {
                        SetMeshMaterial(idleMaterial);
                    }

                    DoPress(player);

                    canInteract = true;
                }
                else
                {
                    if (canInteract)
                    {
                        continue;
                    }

                    if (heldBy != null && heldBy == player)
                    {
                        Release(player);
                        pressed = false;
                        wasPressed = false;
                        heldBy = null;
                    }

                    if (idleMaterial != null)
                    {
                        SetMeshMaterial(idleMaterial);
                    }
                }

                if (pressed)
                {
                    if(pressedMaterial != null)
                    {
                        SetMeshMaterial(pressedMaterial);
                    }
                }
            }
        }


    }

    protected virtual void SetMeshMaterial(Material mat)
    {
        if (currentMeshMaterial == mat)
        {
            return;
        }

        if (mr != null)
        {
            mr.material = mat;
        }

        currentMeshMaterial = mat;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (disabled)
        {
            return;
        }

        if (triggerOnCollide)
        {
            Vector3 relativeVelocity = collision.relativeVelocity;

            Player player = collision.gameObject.GetComponent<Player>();
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();

            if (player == null || rb == null)
            {
                return;
            }

            player.lastVelocity = relativeVelocity.magnitude;

            Press(collision.gameObject);
        }
    }

    protected virtual InteractablePlayerInput PlayerInputCheck(GameObject playerObject)
    {
        Player p = playerObject.GetComponent<Player>();

        PlayerButtonPressed useInput = p.GetUsePressed(); ;

        return new InteractablePlayerInput(useInput.pressed || autoPress == true, useInput.held || autoPress == true);
    }

    /*
     * DoPress function is used to detect whether a player is interacting with the interactable
     * This function will be called when the player is within use area of the interactable
     * This function handles the logic to make useTypes function correctly
    */

    public virtual void DoPress(GameObject playerObject)
    {
        InteractablePlayerInput input = PlayerInputCheck(playerObject);

        bool keyPressed = input.keyPressed;
        bool keyHeld = input.keyHeld;

        switch (useType)
        {
            case UseType.PRESS:
                if (keyPressed)
                {
                    Press(playerObject);

                    if (releaseCooldown > 0 && !disabled)
                    {
                        enableAt = Time.time + releaseCooldown;
                        DisableInteractable();
                    }
                }
                break;
            case UseType.HOLD:
                if (heldBy != null && heldBy != playerObject)
                {
                    return;
                }

                if (keyHeld)
                {
                    pressed = true;

                    if (!wasPressed)
                    {
                        wasPressed = true;
                        Press(playerObject);
                    }

                    heldBy = playerObject;
                }
                else if (wasPressed)
                {
                    wasPressed = false;
                    pressed = false;

                    Release(playerObject);

                    heldBy = null;
                }
                break;
            case UseType.TOGGLE:
                if (keyPressed)
                {
                    if (pressed)
                    {
                        Release(playerObject);
                    }
                    else
                    {
                        Press(playerObject);
                    }

                    pressed = !pressed;
                }
                break;
        }
    }

    protected virtual void Press(GameObject playerObject)
    {
        if (onPressed != null)
        {
            if (useSoundSource != null)
            {
                useSoundSource.Stop();
                useSoundSource.pitch = Random.Range(0.95f, 1.1f);
                useSoundSource.Play();
            }

            onPressed.Invoke(playerObject);
        }
    }

    private void Release(GameObject playerObject)
    {
        if (onReleased != null)
        {
            onReleased.Invoke(playerObject);
        }

        if (releaseCooldown > 0 && !disabled)
        {
            enableAt = Time.time + releaseCooldown;
            DisableInteractable();
        }
    }

    public void EnableInteractable()
    {
        if (!disabled)
        {
            return;
        }

        disabled = false;

        if (onInteractableEnabled != null)
        {
            onInteractableEnabled.Invoke();
        }
    }

    public void DisableInteractable()
    {
        if (disabled)
        {
            return;
        }

        disabled = true;

        if (onInteractableDisabled != null)
        {
            onInteractableDisabled.Invoke();
        }
    }

    public void ToggleInteractable()
    {
        if (disabled)
        {
            EnableInteractable();
        }
        else
        {
            DisableInteractable();
        }
    }

    public void DisableInteractableForTime(float time)
    {
        enableAt = Time.time + time;

        DisableInteractable();
    }

    /*
     * CanInteract function returns the dot product of the relative normalized position of the player and this object, with the forward direction
     * This results in players only being able to interact with useables when they are close enough, and infront of them
    */

    public bool CanInteract(GameObject playerObject)
    {
        if(playerObject == null) { return false; }

        Vector3 playerPosition = playerObject.transform.position;
        Vector3 relativePosition = playerPosition - transform.position;

        float dot = Vector3.Dot(relativePosition.normalized, transform.forward);

        if (dot < 0.7f)
        {
            return false;
        }

        float distance = Vector3.Distance(playerPosition, transform.position);
        return distance < 2;
    }

    public bool IsPressed()
    {
        return pressed;
    }
}

public class InteractablePlayerInput
{
    public bool keyPressed;
    public bool keyHeld;

    public InteractablePlayerInput(bool keyPressed, bool keyHeld)
    {
        this.keyPressed = keyPressed;
        this.keyHeld = keyHeld;
    }
}