using UnityEngine;

public abstract class StanceBase : MonoBehaviour, IStanceDataProvider
{
    [Header("STANCE DATA")]
    [SerializeField] private StanceData _stanceData;

    protected PlayerDataManager _DataManager;
    protected PlayerAttackHandler _AttackHandler;
    protected Health _Health;

    private Transform _center;
    private Renderer _sword;

    private GameObject _currentUltimateGFX;

    public StanceData StanceData => _stanceData;

    public virtual void Init()
    {
        _DataManager = GetComponent<PlayerDataManager>();
        _AttackHandler = GetComponent<PlayerAttackHandler>();
        _Health = GetComponent<Health>();

        _center = _DataManager.ReferenceStruct.Center;
        _sword = _DataManager.ReferenceStruct.Sword;
    }

    public virtual void Enter()
    {
        Instantiate(_stanceData.SwapVFX, _center);
        _sword.material.color = _stanceData.Color;
    }

    public virtual void Exit()
    {
        // Exit logic is not commonly shared.
    }

    public virtual void CastUltimate()
    {
        _DataManager.UltimateStruct.IsUltimateActive = true;
        _currentUltimateGFX = Instantiate(_stanceData.UltimateGFX, _center);
        Invoke(nameof(DeactivateUltimate), _stanceData.UltimateDuration);
        
        // Play audio
    }

    public virtual void DeactivateUltimate()
    {
        _DataManager.UltimateStruct.IsUltimateActive = false;
        Destroy(_currentUltimateGFX);
        
        // Stop audio
    }
}
