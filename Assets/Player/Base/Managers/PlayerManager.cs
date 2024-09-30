using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("PLAYER STAT REFERENCE")]
    [SerializeField] private PlayerStats _stats;

    [Header("PLAYER UI REFERENCES")]
    [SerializeField] private StanceUI _stanceUI;

    private PlayerInputHandler _inputHandler;
    private PlayerAnimatorManager _animatorManager;
    private PlayerStanceManager _stanceManager;
    private PlayerDataManager _dataManager;
    private PlayerLockOn _lockOn;
    private PlayerLocomotion _locomotion;
    private PlayerAttackHandler _attackHandler;
    private Health _health;
    private Mana _mana;
    private Souls _souls;
    private MovementTracking _movementTracking;
    private CameraController _cameraManager;
    
    public void Init()
    {
        DontDestroyOnLoad(gameObject);

        _stats.StatChanged += PlayerManager_StatChanged;

        _inputHandler = GetComponent<PlayerInputHandler>();
        _animatorManager = GetComponent<PlayerAnimatorManager>();
        _stanceManager = GetComponent<PlayerStanceManager>();
        _dataManager = GetComponent<PlayerDataManager>();
        _lockOn = GetComponent<PlayerLockOn>();
        _locomotion = GetComponent<PlayerLocomotion>();
        _attackHandler = GetComponent<PlayerAttackHandler>();
        _health = GetComponent<Health>();
        _mana = GetComponent<Mana>();
        _souls = GetComponent<Souls>();
        _movementTracking = GetComponent<MovementTracking>();
        _cameraManager = CameraController.Instance;

        _stanceUI.Init();

        _inputHandler.Init();
        _animatorManager.Init();
        _stanceManager.Init();
        _lockOn.Init();
        _locomotion.Init();
        _attackHandler.Init();
        _health.Init(_stats.GetBaseStat(Stat.MaxHealth), _stats.GetCurrentStat(Stat.Health));
        _mana.Init(_stats.GetBaseStat(Stat.MaxMana), _stats.GetCurrentStat(Stat.Mana));
        _souls.Init();
        _movementTracking.Init();
        _cameraManager.Init(transform);

        _health.AddConstantValue(_stats.GetCurrentStat(Stat.HealthRegen), 1);
        _mana.AddConstantValue(_stats.GetCurrentStat(Stat.ManaRegen), 1);
        _health.DamageReduction = _stats.GetCurrentStat(Stat.DamageReduction);
        _health.EvasionChance = _stats.GetCurrentStat(Stat.EvasionChance);

        _health.CurrentValueUpdated += PlayerManager_CurrentHealthUpdated;
        _mana.CurrentValueUpdated += PlayerManager_CurrentManaUpdated;
    }

    public void Tick(float delta)
    {
        _inputHandler.Tick();
        _locomotion.Tick();
        _attackHandler.Tick(delta);
        _movementTracking.Tick(delta);
    }

    public void FixedTick(float delta)
    {
        Vector3 xDirection = _cameraManager._CameraTransform.right;
        Vector3 yDirection = _cameraManager._CameraTransform.forward;
        float leftStickX = _inputHandler._LeftStickX;
        float leftStickY = _inputHandler._LeftStickY;
        float rightStickX = _inputHandler._RightStickX;
        float rightStickY = _inputHandler._RightStickY;

        Transform lockOnTargetTransform = _dataManager.LockOnStruct.LockOnTargetTransform;
        bool lockedOn = _dataManager.LockOnStruct.LockedOn;

        _locomotion.FixedTick(delta, xDirection, yDirection, leftStickX, leftStickY, lockOnTargetTransform, lockedOn);
        _lockOn.FixedTick();
        _cameraManager.FixedTick(delta, rightStickX, rightStickY, lockOnTargetTransform, lockedOn);
    }

    private void PlayerManager_StatChanged(Stat stat, float value)
    {
        switch (stat)
        {
            case Stat.HealthRegen:
                _health.RestorationModifier = value;
                break;

            case Stat.ManaRegen:
                _mana.RestorationModifier = value;
                break;

            case Stat.DamageReduction:
                _health.DamageReduction = value;
                break;

            case Stat.EvasionChance:
                _health.EvasionChance = value;
                break;
        }
    }

    private void PlayerManager_CurrentHealthUpdated(float currentValue)
    {
        _stats.SetCurrentStat(Stat.Health, currentValue);
    }

    private void PlayerManager_CurrentManaUpdated(float currentValue)
    {
        _stats.SetCurrentStat(Stat.Mana, currentValue);
    }
}
