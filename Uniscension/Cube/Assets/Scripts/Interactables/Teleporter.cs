using UnityEngine;

/*
 * This class inherits Interactable
 * Teleporters are used to teleport players from one location to another
*/

public class Teleporter : Interactable
{

    public float exitPositionDistance = 1f;
    public bool disableOnTeleport = true;

    private GameObject passObject;
    private ParticleSystem passEmitter;

    // Use this for initialization
    protected override void Start()
    {
        Transform passTransform = transform.Find("Purple Portal Pass-Through");

        if(passTransform != null)
        {
            passObject = passTransform.gameObject;
            passEmitter = passObject.GetComponent<ParticleSystem>();
        }

        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {

        base.Update();
    }

    protected override void Press(GameObject playerObject)
    {
        if (passEmitter != null)
        {
            passEmitter.Clear();
            passEmitter.Play();
        }

        base.Press(playerObject);
    }

    public void TeleportTo(GameObject playerObject)
    {
        Vector3 exitPosition = transform.position + (transform.forward * exitPositionDistance);
        playerObject.transform.position = exitPosition;

        Player player = playerObject.GetComponent<Player>();
        if (player != null)
        {
            player.CorrectCubeSide();
            player.CorrectPosition();
            player.CorrectAngle(180);
        }

        if (disableOnTeleport)
        {
            DisableInteractableForTime(1);
        }

        if (player.lastVelocity != 0)
        {
            Rigidbody rb = playerObject.GetComponent<Rigidbody>();

            if(rb != null)
            {
                Vector3 transformedVelocity = transform.forward * player.lastVelocity;

                rb.velocity = transformedVelocity;
            }

            player.lastVelocity = 0;
        }

        if(passEmitter != null)
        {
            passEmitter.Clear();
            passEmitter.Play();
        }
    }
}
