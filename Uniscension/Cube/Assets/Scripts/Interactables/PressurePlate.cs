using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : Interactable {

    public GameObject buttonObject { get; private set; }

    private MeshRenderer buttonMesh;
    private float lastTrigger = 0f;

	// Use this for initialization
	protected override void Start () {
        Transform buttonTransform = transform.Find("Button");
        if(buttonTransform != null)
        {
            buttonObject = buttonTransform.gameObject;
            buttonMesh = buttonObject.GetComponent<MeshRenderer>();
        }

        base.Start();
	}
	
	// Update is called once per frame
	protected override void Update () {
        useType = UseType.HOLD;
        autoPress = true;

        if(buttonMesh != null && currentMeshMaterial != null)
        {
            buttonMesh.material = currentMeshMaterial;
        }

        if (IsPressed())
        {

        }
        else
        {

        }

        base.Update();
	}

    protected override InteractablePlayerInput PlayerInputCheck(GameObject playerObject)
    {
        bool pressed = false;

        Player p = playerObject.GetComponent<Player>();
        if(p != null && buttonObject != null) {
            pressed = p.GroundedTo() == buttonObject;
        }

        if(pressed && !IsPressed()){
            lastTrigger = Time.time;
        }

        if(!pressed && lastTrigger != 0)
        {
            if(Time.time - lastTrigger < 0.2)
            {
                pressed = true;
            }
        }

        return new InteractablePlayerInput(pressed, pressed);
    }
}
