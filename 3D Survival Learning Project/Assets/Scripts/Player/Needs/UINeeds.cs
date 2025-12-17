using UnityEngine;
using UnityEngine.UI;

public class UINeeds : MonoBehaviour
{
    [SerializeField] private Image _healthBar;
    [SerializeField] private Image _hungerBar;
    [SerializeField] private Image _energyBar;

    private NeedsManager _needsManager;

    private void Awake()
    {
        _needsManager = NeedsManager.instance;
    }

    private void Update()
    {
        _healthBar.fillAmount = _needsManager.Health.GetPercentage();
        _hungerBar.fillAmount = _needsManager.Hunger.GetPercentage();
        _energyBar.fillAmount = _needsManager.Energy.GetPercentage();
    }
}
