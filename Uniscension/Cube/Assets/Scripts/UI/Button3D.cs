using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button3D : MonoBehaviour {

    [System.Serializable]
    public class CustomUIEvent : UnityEvent { }

    public string text;
    public CustomUIEvent onPressed;
    public bool controllerInput = false;
    public int controllerInputIndex = 0;

    [Header("Appearance")]
    public Material idleMaterial;
    public Material hoveredMaterial;
    public Material selectedMaterial;
    public Material clickedMaterial;

    public bool hovered { get; private set; }
    public bool clicked { get; private set; }
    public bool selected { get; private set; }

    private bool wasDPad = false;
    private MeshRenderer mr;
    private ParticleSystem clickParticleSystem;
    private float lastHover = 0;
    private Menu3D menu;

    // Use this for initialization
    void Start () {
		if(text == null)
        {
            text = "N/A";
        }

        mr = gameObject.GetComponent<MeshRenderer>();

        Transform particleTransform = transform.Find("Particles");
        if(particleTransform != null)
        {
            clickParticleSystem = particleTransform.gameObject.GetComponent<ParticleSystem>();
        }
    }
	
	// Update is called once per frame
	void Update () {
        if(Time.time - lastHover > 0.01f)
        {
            SetMaterial(idleMaterial);
        }

        if(menu != null)
        {
            int controllerSelect = menu.controllerSelect;

            selected = controllerInputIndex == controllerSelect;

            if (selected)
            {
                SetMaterial(selectedMaterial);

                if (Input.GetButtonDown("ControllerSelect"))
                {
                    Press();
                    clicked = true;
                }
                else
                {
                    clicked = false;
                }
            }
        }
    }

    public void OnMouseOver()
    {
        hovered = true;
        SetMaterial(hoveredMaterial);

        if (Input.GetMouseButton(0))
        {
            if (!clicked)
            {
                Press();
            }
            clicked = true;
            SetMaterial(clickedMaterial);
        }
        else
        {
            clicked = false;
        }

        lastHover = Time.time;
    }

    public void Press()
    {
        if (onPressed != null)
        {
            onPressed.Invoke();

            if(clickParticleSystem != null)
            {
                clickParticleSystem.Stop();
                clickParticleSystem.Play();
            }
        }
    }

    private void SetMaterial(Material material)
    {
        if(material == null || mr == null) { return; }

        mr.material = material;
    }

    public void SetMenu(Menu3D menu)
    {
        this.menu = menu;
    }
}
