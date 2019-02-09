using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu3D : MonoBehaviour {

    public int controllerSelect { get; private set; }
    public int elements { get; private set; }

    private bool wasDPad;

    // Use this for initialization
    void Start () {
		for(int i = 0; i < transform.childCount; i++)
        {
            GameObject obj = transform.GetChild(i).gameObject;

            Button3D button = obj.GetComponent<Button3D>();
            if(button != null)
            {
                button.SetMenu(this);
                elements++;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        float dpad = Input.GetAxis("DPadVertical");

        if (dpad != 0)
        {
            if (!wasDPad)
            {
                wasDPad = true;

                if (dpad > 0)
                {
                    controllerSelect--;
                }
                else
                {
                    controllerSelect++;
                }
            }
        }
        else
        {
            wasDPad = false;
        }

        controllerSelect = Mathf.Clamp(controllerSelect, 0, elements - 1);
    }
}
