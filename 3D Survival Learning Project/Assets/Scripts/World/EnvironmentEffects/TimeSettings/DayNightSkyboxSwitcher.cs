using UnityEngine;

public class DayNightSkyboxSwitcher : MonoBehaviour
{
    [Header("Skybox Materials")]
    [SerializeField] private Material _daySkybox;
    [SerializeField] private Material _nightSkybox;

    [Header("Refernces")]
    [SerializeField] private DayNightCycle _timeController;

    private bool _isDay = true;

    private void Start()
    {
        RenderSettings.skybox = _daySkybox;
        _isDay = true;
    }

    private void Update()
    {
        HandleSkyboxState(_timeController.TimeOfDay);
    }

    private void HandleSkyboxState(float time)
    {
        switch (time)
        {
            case >= 0.8f and < 0.85f:
                if (!_isDay) SwitchToDay();
                float sunsetProgress = (time - 0.8f) / 0.05f;
                SetSkyboxExposure(1f - sunsetProgress);
                break;
            case >= 0.85f and < 0.9f:
                if (_isDay) SwitchToNight();
                float nightRiseProgress = (time - 0.85f) / 0.05f;
                SetSkyboxExposure(nightRiseProgress);
                break;
            case >= 0.2f and < 0.25f:
                if (_isDay) SwitchToNight();
                float nightFadeProgress = (time - 0.2f) / 0.05f;
                SetSkyboxExposure(1f - nightFadeProgress);
                break;
            case >= 0.25f and < 0.3f:
                if (!_isDay) SwitchToDay();
                float dayRiseProgress = (time - 0.25f) / 0.05f;
                SetSkyboxExposure(dayRiseProgress);
                break;
            default:
                SetSkyboxExposure(1f);
                break;
        }
    }
    private void SwitchToDay()
    {
        RenderSettings.skybox = _daySkybox;
        _isDay = true;
    }

    private void SwitchToNight()
    {
        RenderSettings.skybox = _nightSkybox;
        _isDay = false;
    }

    private void SetSkyboxExposure(float value)
    {
        if (RenderSettings.skybox.HasProperty("_Exposure"))
        {
            RenderSettings.skybox.SetFloat("_Exposure", value);
        }
        else if (RenderSettings.skybox.HasProperty("_Tint"))
        {
            RenderSettings.skybox.SetColor("_Tint", Color.Lerp(Color.black, Color.white, value));
        }
    }
}
