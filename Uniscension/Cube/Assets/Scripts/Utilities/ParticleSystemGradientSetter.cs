using UnityEngine;

public class ParticleSystemGradientSetter : MonoBehaviour
{
    [SerializeField] [Tooltip("The gradient that will be assigned to 'Color' in 'ColorOverLifetimeModule'")] private Gradient gradient;

    private void OnValidate()
    {
        SetColourOverLifetime();
    }

    private void Start()
    {
        SetColourOverLifetime();
    }

    private void SetColourOverLifetime()
    {
        ParticleSystem.ColorOverLifetimeModule colourOverLifetime = gameObject.GetComponent<ParticleSystem>().colorOverLifetime;
        if (!colourOverLifetime.enabled)
        {
            colourOverLifetime.enabled = true;
        }
        colourOverLifetime.color = gradient;
    }
}