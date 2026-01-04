using UnityEngine;
using UnityEngine.Rendering;

public class UnderwaterEffect : MonoBehaviour
{
    [Header("Water Settings")]
    public Color underwaterColor = new Color(0.1f, 0.3f, 0.35f, 0.6f);
    public float underwaterDensity = 0.05f;

    private bool defaultFogState;
    private Color defaultFogColor;
    private float defaultFogDensity;
    private Material skyboxDefault;

    private void Start()
    {
        defaultFogState = RenderSettings.fog;
        defaultFogColor = RenderSettings.fogColor;
        defaultFogDensity = RenderSettings.fogDensity;
        skyboxDefault = RenderSettings.skybox;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            EnableUnderwaterEffect();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            DisableUnderwaterEffect();
        }
    }

    private void EnableUnderwaterEffect()
    {

        RenderSettings.fog = true;
        RenderSettings.fogColor = underwaterColor;
        RenderSettings.fogDensity = underwaterDensity;
        RenderSettings.fogMode = FogMode.Exponential;
        RenderSettings.skybox = null;
    }

    private void DisableUnderwaterEffect()
    {
        RenderSettings.fog = defaultFogState;
        RenderSettings.fogColor = defaultFogColor;
        RenderSettings.fogDensity = defaultFogDensity;
        RenderSettings.skybox = skyboxDefault;
    }
}
