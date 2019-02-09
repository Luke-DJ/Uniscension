using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : Interactable {

    private MeshRenderer buttonMesh1;
    private MeshRenderer buttonMesh2;

	// Use this for initialization
	protected override void Start () {
        Transform buttonTransform = transform.Find("button");

        if(buttonTransform != null)
        {
            Transform buttonTransform1 = buttonTransform.Find("Sphere001");
            if(buttonTransform1 != null)
            {
                buttonMesh1 = buttonTransform1.GetComponent<MeshRenderer>();
            }
            Transform buttonTransform2 = buttonTransform.Find("Sphere002");
            if (buttonTransform1 != null)
            {
                buttonMesh2 = buttonTransform2.GetComponent<MeshRenderer>();
            }
        }
        
        base.Start();
	}
	
	// Update is called once per frame
	protected override void Update () {
        
        base.Update();
	}

    protected override void SetMeshMaterial(Material mat)
    {
        if(buttonMesh1 != null)
        {
            buttonMesh1.material = mat;
        }
        if(buttonMesh2 != null)
        {
            buttonMesh2.material = mat;
        }
        base.SetMeshMaterial(mat);
    }
}
