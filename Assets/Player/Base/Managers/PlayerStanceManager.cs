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

    // References
    private PlayerInputHandler _inputHandler;
    private PlayerDataManager _dataManager;

    // StanceBase Related
    [SerializeField] private List<StanceBase> _stances = new();
    private StanceBase _currentStance;
    private int _currentIndex = 0;

    [Header("VARIABLES")]
    [SerializeField] private float _swapCD = 0.5f;

    private AudioSystem _audioSystem;

    public delegate void StanceSwappedDelegate(StanceType currentStance, StanceType nextStance);
    public event StanceSwappedDelegate StanceSwapped;

    public void Init()
    {
        _inputHandler = GetComponent<PlayerInputHandler>();
        _dataManager = GetComponent<PlayerDataManager>();

        _audioSystem = AudioSystem.Instance;

        _inputHandler.UltimateInputPerformed += PlayerStanceManager_UltimateInput_Performed;
        _inputHandler.SwapStanceInputPerformed += PlayerStanceManager_SwapStanceInput_Performed;

        _stances.AddRange(GetComponents<StanceBase>());
        foreach (var stance in _stances) stance.Init();

        _currentStance = _stances[0];
        SwapToStance(_currentStance);
    }

    private void PlayerStanceManager_UltimateInput_Performed()
    {
        if (!_dataManager.UltimateStruct.IsUltimateActive)
        {
            _currentStance.CastUltimate();
        }
    }

    private void PlayerStanceManager_SwapStanceInput_Performed()
    {
        _inputHandler.SwapStanceInputPerformed -= PlayerStanceManager_SwapStanceInput_Performed;
        Invoke(nameof(ResubscribeToSwapStance), _swapCD);

        _currentIndex = Helpers.GetIndexInBounds(_currentIndex, 1, _stances.Count);
        SwapToStance(_stances[_currentIndex]);
    }

    private void SwapToStance(StanceBase currentStance)
    {
        foreach (var stance in _stances) stance.Exit();
        _currentStance = currentStance;
        currentStance.Enter();

        StanceType nextStance = _stances[Helpers.GetIndexInBounds(_currentIndex, 1, _stances.Count)].StanceData.StanceType;
        OnStanceSwapped(currentStance.StanceData.StanceType, nextStance);
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
