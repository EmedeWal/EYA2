using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStanceManager : SingletonBase
{
    #region Singleton
    public static PlayerStanceManager Instance;

    public override void SingletonSetup()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    // References
    private PlayerInputHandler _inputHandler;
    private PlayerDataManager _dataManager;

    // StanceBase Related
    private List<StanceBase> _stances = new();
    private StanceBase _currentStance;
    private int _currentIndex = 0;

    [Header("VARIABLES")]
    [SerializeField] private float _swapCD = 0.5f;

    private AudioSystem _audioSystem;

    public delegate void StanceUnlockedDelegate(StanceType stanceUnlocked);
    public event StanceUnlockedDelegate StanceUnlocked;

    public delegate void StanceSwappedDelegate(StanceType currentStance, StanceType nextStance);
    public event StanceSwappedDelegate StanceSwapped;

    public void Init()
    {
        _inputHandler = GetComponent<PlayerInputHandler>();
        _dataManager = GetComponent<PlayerDataManager>();

        _audioSystem = AudioSystem.Instance;

        _inputHandler.UltimateInputPerformed += PlayerStanceManager_UltimateInput_Performed;
        _inputHandler.SwapStanceInputPerformed += PlayerStanceManager_SwapStanceInput_Performed;
    }

    public void UnlockStance(StanceType stanceTypeToUnlock)
    {
        StanceBase[] stances = GetComponents<StanceBase>();
        foreach (StanceBase stance in stances)
        {
            StanceType stanceTypeToCheck = stance.StanceData.StanceType;
            if (stanceTypeToCheck == stanceTypeToUnlock)
            {
                if (_stances.Contains(stance))
                {
                    continue;
                }
                else
                {
                    stance.Init();
                    _stances.Add(stance);
                    Helpers.SortByStanceType(_stances);
                    _currentIndex = _stances.IndexOf(stance);
                    OnStanceUnlocked(stanceTypeToUnlock);
                    SwapToStance(stance);
                }
            }
        }
    }


    private void PlayerStanceManager_UltimateInput_Performed()
    {
        if (_currentStance != null && !_dataManager.UltimateStruct.IsUltimateActive)
        {
            _currentStance.CastUltimate();
        }
    }

    private void PlayerStanceManager_SwapStanceInput_Performed()
    {
        if (_stances.Count > 1)
        {
            _inputHandler.SwapStanceInputPerformed -= PlayerStanceManager_SwapStanceInput_Performed;
            Invoke(nameof(ResubscribeToSwapStance), _swapCD);

            _currentIndex = Helpers.GetIndexInBounds(_currentIndex, 1, _stances.Count);
            SwapToStance(_stances[_currentIndex]);
        }
    }

    private void SwapToStance(StanceBase currentStance)
    {
        foreach (var stance in _stances) stance.Exit();
        _currentStance = currentStance;
        currentStance.Enter();

        StanceType nextStance;
        if (_stances.Count > 1)
        {
            nextStance = _stances[Helpers.GetIndexInBounds(_currentIndex, 1, _stances.Count)].StanceData.StanceType;
        }
        else
        {
            nextStance = StanceType.None;
        }

        OnStanceSwapped(currentStance.StanceData.StanceType, nextStance);
    }

    private void OnStanceUnlocked(StanceType unlockedStance)
    {
        StanceUnlocked?.Invoke(unlockedStance);
    }

    private void OnStanceSwapped(StanceType currentStance, StanceType nextStance)
    {
        StanceSwapped?.Invoke(currentStance, nextStance);
    }

    private void ResubscribeToSwapStance()
    {
        _inputHandler.SwapStanceInputPerformed += PlayerStanceManager_SwapStanceInput_Performed;
    }
}
