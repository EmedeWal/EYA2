using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStanceManager : MonoBehaviour
{
    //// DISCARD LATER. TESTING PURPOSES ONLY. ALONG WITH START FUNCTION
    //public bool UnlockOnStart;

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

    private void OnStanceSwapped(StanceType currentStance, StanceType nextStance)
    {
        StanceSwapped?.Invoke(currentStance, nextStance);
    }


    //public void UnlockVampireStance()
    //{
    //    IStance[] stances = GetComponents<IStance>();
    //    UnlockStance(stances[0]);
    //}

    //public void UnlockOrcStance()
    //{
    //    IStance[] stances = GetComponents<IStance>();
    //    UnlockStance(stances[1]);
    //}

    //public void UnlockGhostStance()
    //{
    //    IStance[] stances = GetComponents<IStance>();
    //    UnlockStance(stances[2]);
    //}

    //private void UnlockStance(IStance stance)
    //{
    //    if (!_stances.Contains(stance))
    //    {
    //        _stances.Add(stance);
    //        SwapStance(stance);
    //    }
    //}

    //private void PlayerStanceManager_UnlockStance(StanceType stanceType)
    //{
    //    switch (stanceType)
    //    {
    //        case StanceType.Vampire:
    //            UnlockVampireStance();
    //            break;

    //        case StanceType.Orc:
    //            UnlockOrcStance();
    //            break;

    //        case StanceType.Ghost: 
    //            UnlockGhostStance();
    //            break;
    //    }
    //}


    //public void OnStanceSwapped()
    //{
    //    StanceSwappedDelegate?.Invoke();
    //}

    private void ResubscribeToSwapStance()
    {
        _inputHandler.SwapStanceInputPerformed += PlayerStanceManager_SwapStanceInput_Performed;
    }
}
