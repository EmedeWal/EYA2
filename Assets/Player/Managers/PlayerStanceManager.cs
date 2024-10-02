using System.Collections.Generic;
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

    private PlayerInputHandler _inputHandler;
    private PlayerDataManager _dataManager;
    private Mana _mana;

    private List<Stance> _stances = new();
    private Stance _currentStance;
    private int _currentIndex = 0;

    [Header("VARIABLES")]
    [SerializeField] private float _swapCD = 1f;
    private float _swapCooldownRemaining = 0f;

    public delegate void StanceSwappedDelegate(StanceType currentStance, StanceType nextStance);
    public event StanceSwappedDelegate StanceSwapped;

    public delegate void SwapCooldownUpdateDelegate(float progress);
    public event SwapCooldownUpdateDelegate SwapCooldownUpdated;

    public void Init()
    {
        _inputHandler = GetComponent<PlayerInputHandler>();
        _dataManager = GetComponent<PlayerDataManager>();
        _mana = GetComponent<Mana>();   

        _inputHandler.UltimateInputPerformed += PlayerStanceManager_UltimateInputPerformed;
        _inputHandler.SwapStanceInputPerformed += PlayerStanceManager_SwapStanceInputPerformed;

        _stances.AddRange(GetComponents<Stance>());
        foreach (var stance in _stances) stance.Init();

        _currentStance = _stances[0];
        SwapToStance(_currentStance);
    }

    public void Tick(float delta)
    {
        _currentStance.Tick(delta);

        if (_swapCooldownRemaining > 0)
        {
            _swapCooldownRemaining -= delta;
            _swapCooldownRemaining = Mathf.Max(_swapCooldownRemaining, 0f);

            float progress = 1 - (_swapCooldownRemaining / _swapCD);
            OnSwapCooldownUpdated(progress);
        }
    }

    public void Cleanup()
    {
        _inputHandler.UltimateInputPerformed -= PlayerStanceManager_UltimateInputPerformed;
        _inputHandler.SwapStanceInputPerformed -= PlayerStanceManager_SwapStanceInputPerformed;

        _currentStance.CleanUp();
    }

    public void AddPerk(PerkData perkData, StanceType perkType)
    {
        foreach (var stance in _stances)
        {
            StanceType stanceType = stance.StanceData.StanceType;

            if (stanceType == perkType)
            {
                stance.AddPerk(perkData);
            }
        }
    }

    private void PlayerStanceManager_UltimateInputPerformed()
    {
        if (!_dataManager.UltimateStruct.IsUltimateActive && _mana.AtMaxValue())
        {
            _currentStance.CastUltimate();
        }
    }

    private void PlayerStanceManager_SwapStanceInputPerformed()
    {
        if (_swapCooldownRemaining > 0) return;

        _swapCooldownRemaining = _swapCD;

        _currentIndex = Helpers.GetIndexInBounds(_currentIndex, 1, _stances.Count);
        SwapToStance(_stances[_currentIndex]);
    }

    private void SwapToStance(Stance newStance)
    {
        if (_currentStance != null) _currentStance.Exit();
        _currentStance = newStance;
        newStance.Enter();

        StanceType nextStance = _stances[Helpers.GetIndexInBounds(_currentIndex, 1, _stances.Count)].StanceData.StanceType;
        OnStanceSwapped(newStance.StanceData.StanceType, nextStance);
    }

    private void OnStanceSwapped(StanceType currentStance, StanceType nextStance)
    {
        StanceSwapped?.Invoke(currentStance, nextStance);
    }

    private void OnSwapCooldownUpdated(float progress)
    {
        SwapCooldownUpdated?.Invoke(progress);
    }
}
