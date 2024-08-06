using System.Collections.Generic;
using UnityEngine;

public class PlayerStanceManager : MonoBehaviour
{
    // DISCARD LATER. TESTING PURPOSES ONLY. ALONG WITH START FUNCTION
    public bool UnlockOnStart;

    // References
    private PlayerInputManager _inputManager;
    private PlayerDataManager _dataManager;
    private Mana _mana;

    // Stance Related
    private List<IStance> _stances = new List<IStance>();
    private IStance _currentStance;
    private int _stancePosition = 0;

    [Header("VARIABLES")]
    [SerializeField] private float _swapCD = 0.5f;
    private bool _canSwap = true;

    [Header("AUDIO")]
    [SerializeField] private AudioSource _audioSource;

    public delegate void PlayerStanceManager_StanceSwap();
    public static event PlayerStanceManager_StanceSwap StanceSwap;

    private void Awake()
    {
        _inputManager = GetComponent<PlayerInputManager>();
        _dataManager = GetComponent<PlayerDataManager>();
        _mana = GetComponent<Mana>();
    }

    private void Start()
    {
        if (UnlockOnStart)
        {
            UnlockVampireStance();
            UnlockOrcStance();
            UnlockGhostStance();
        }
    }

    private void OnEnable()
    {
        StancePurchaseMenu.UnlockStance += PlayerStanceManager_UnlockStance;
        _inputManager.SwapStanceInput_Performed += PlayerStanceManager_SwapStanceInput_Performed;
        _inputManager.UltimateInput_Performed += PlayerStanceManager_UltimateInput_Performed;
    }

    private void OnDisable()
    {
        StancePurchaseMenu.UnlockStance -= PlayerStanceManager_UnlockStance;
        _inputManager.SwapStanceInput_Performed -= PlayerStanceManager_SwapStanceInput_Performed;
        _inputManager.UltimateInput_Performed -= PlayerStanceManager_UltimateInput_Performed;
    }

    public void UnlockVampireStance()
    {
        IStance[] stances = GetComponents<IStance>();
        UnlockStance(stances[0]);
    }

    public void UnlockOrcStance()
    {
        IStance[] stances = GetComponents<IStance>();
        UnlockStance(stances[1]);
    }

    public void UnlockGhostStance()
    {
        IStance[] stances = GetComponents<IStance>();
        UnlockStance(stances[2]);
    }

    private void UnlockStance(IStance newStance)
    {
        if (!_stances.Contains(newStance))
        {
            _stances.Add(newStance);
            SwapStance(newStance);
        }
    }

    private void PlayerStanceManager_UnlockStance(StanceType stanceType)
    {
        switch (stanceType)
        {
            case StanceType.Vampire:
                UnlockVampireStance();
                break;

            case StanceType.Orc:
                UnlockOrcStance();
                break;

            case StanceType.Ghost: 
                UnlockGhostStance();
                break;
        }
    }

    private void PlayerStanceManager_SwapStanceInput_Performed()
    {
        if (_stances.Count > 1 && _canSwap)
        {
            DetermineStance();
            OnStanceSwap();
        }
    }

    private void PlayerStanceManager_UltimateInput_Performed()
    {
        if (_stances.Count != 0 && _mana.AtMaxValue() && !_dataManager.GetUltimateActivate())
        {
            _currentStance.CastUltimate();
            _mana.SpendMana(100);
        }
    }

    private void DetermineStance()
    {
        Invoke(nameof(ResetSwap), _swapCD);
        _audioSource.Play();
        _canSwap = false;

        int lastPosition = _stances.Count - 1;

        _stancePosition += 1;
        if (_stancePosition < 0) _stancePosition = lastPosition;
        else if (_stancePosition > lastPosition) _stancePosition = 0;

        SwapStance(_currentStance = _stances[_stancePosition]);
    }

    private void SwapStance(IStance newStance)
    {
        foreach (var stance in _stances) stance.Exit();
        _currentStance = newStance;
        newStance.Enter();
    }

    private void OnStanceSwap()
    {
        StanceSwap?.Invoke();
    }

    private void ResetSwap()
    {
        _canSwap = true;
    }
}
