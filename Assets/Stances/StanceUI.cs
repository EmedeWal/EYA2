using UnityEngine.UI;
using UnityEngine;

public class StanceUI : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private PlayerStanceManager _playerStanceManager;
    [SerializeField] private Mana _mana;

    [Header("IMAGE")]
    [SerializeField] private Image _foredrop;

    [Header("STANCE DATA")]
    [SerializeField] private StanceData[] _stanceData;
    private StanceIcon[] _stanceIcons;
    private float _maxMana;
    private int _currentStanceIndex = 1;
    private int _nextStanceIndex = 0;

    public void Init()
    {
        _stanceIcons = GetComponentsInChildren<StanceIcon>();

        _playerStanceManager.StanceSwapped += StanceUI_StanceSwapped;
        _playerStanceManager.SwapCooldownUpdated += SwapCooldownUpdated;

        _mana.MaxValueInitialized += StanceUI_MaxValueInitialised;
        _mana.CurrentValueUpdated += StanceUI_CurrentValueUpdated;
    }

    private void StanceUI_StanceSwapped(StanceType currentStance, StanceType nextStance)
    {
        UpdateStanceUI(currentStance, _currentStanceIndex);
        UpdateStanceUI(nextStance, _nextStanceIndex);
    }

    private void StanceUI_MaxValueInitialised(float maxValue)
    {
        _maxMana = maxValue;
    }

    private void StanceUI_CurrentValueUpdated(float currentValue)
    {
        _stanceIcons[_currentStanceIndex].Background.fillAmount = currentValue / _maxMana;
    }

    private void SwapCooldownUpdated(float progress)
    {
        _foredrop.fillAmount = 1 - progress;
    }

    private void UpdateStanceUI(StanceType stanceType, int index)
    {
        StanceData currentData = null;

        foreach (var stanceData in _stanceData)
        {
            if (stanceData.StanceType == stanceType)
            {
                currentData = stanceData;
                break;
            }
        }

        _stanceIcons[index].Background.color = currentData.Color;
        _stanceIcons[index].Icon.sprite = currentData.IconSprite;
    }
}
