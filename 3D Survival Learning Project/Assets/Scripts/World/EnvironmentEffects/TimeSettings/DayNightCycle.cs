using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Header("TimeSettings")]
    [Tooltip("Day duration in seconds")]
    [SerializeField] private float _dayDurationInSeconds = 600f;
    [Range(0f, 1f)]
    [SerializeField] private float _timeOfDay = 0.34f;
    [HideInInspector]public float TimeOfDay =>_timeOfDay;

    [Header("Sun Settings")]
    [SerializeField] private Light _sunLight;
    [SerializeField] private Gradient _sunColor;
    [SerializeField] private AnimationCurve _sunIntensity;

    [Header("Ambient Settings (world light)")]
    [SerializeField] private Gradient _ambientColor;
    [SerializeField] private Gradient _fogColor;

    [Header("References")]
    [SerializeField] private PlayerController _player;

    private void Update()
    {
        _timeOfDay += Time.deltaTime / _dayDurationInSeconds;
        if(_timeOfDay >= 1f) _timeOfDay = 0f;

        UpdateSunPosition();
        UpdateLighting();
    }

    private void UpdateSunPosition()
    {
        float sunRotation = (_timeOfDay * 360f) - 90f;
        _sunLight.transform.localRotation = Quaternion.Euler(sunRotation, 170f, 0f);
    }

    private void UpdateLighting()
    {
        if (_player != null && _player.isInWater) return;
        _sunLight.color = _sunColor.Evaluate(_timeOfDay);
        _sunLight.intensity = _sunIntensity.Evaluate(_timeOfDay);
        RenderSettings.ambientLight = _ambientColor.Evaluate(_timeOfDay);
        RenderSettings.fogColor = _fogColor.Evaluate(_timeOfDay);
    }
}
