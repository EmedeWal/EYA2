using UnityEngine;

public class PlayerStatManager : MonoBehaviour
{
    [Header("PLAYER STAT REFERENCE")]
    [SerializeField] private PlayerStats _stats;

    [Header("ATTACK DATA REFERENCES")]
    [SerializeField] private BaseAttackData _lightAttackData;
    [SerializeField] private BaseAttackData _heavyAttackData;

    private PlayerAnimatorManager _animatorManager;
    private PlayerLocomotion _locomotion;
    private PlayerHealth _health;
    private Mana _mana;

    public void Init()
    {
        _stats.StatChanged += PlayerStatManager_StatChanged;

        _animatorManager = GetComponent<PlayerAnimatorManager>();
        _locomotion = GetComponent<PlayerLocomotion>();
        _health = GetComponent<PlayerHealth>();
        _mana = GetComponent<Mana>();

        _health.Init(_stats.GetBaseStat(Stat.MaxHealth), _stats.GetCurrentStat(Stat.Health));
        _mana.Init(_stats.GetBaseStat(Stat.MaxMana), _stats.GetCurrentStat(Stat.Mana));

        _health.AddConstantValue(_stats.GetCurrentStat(Stat.HealthRegen));
        _mana.AddConstantValue(_stats.GetCurrentStat(Stat.ManaRegen));

        _lightAttackData.Damage = _stats.GetCurrentStat(Stat.LightAttackDamage);
        _heavyAttackData.Damage = _stats.GetCurrentStat(Stat.HeavyAttackDamage);

        _animatorManager.MovementSpeed = _stats.GetCurrentStat(Stat.MovementSpeedModifier);
        _animatorManager.AttackSpeed = _stats.GetCurrentStat(Stat.AttackSpeed);
        _locomotion.MovementSpeed = _stats.GetCurrentStat(Stat.MovementSpeed);
        _health.DamageReduction = _stats.GetCurrentStat(Stat.DamageReduction);
        _health.EvasionChance = _stats.GetCurrentStat(Stat.EvasionChance);

        _health.CurrentValueUpdated += PlayerManager_CurrentHealthUpdated;
        _mana.CurrentValueUpdated += PlayerManager_CurrentManaUpdated;
    }

    public void LateTick(float delta)
    {
        _health.LateTick(delta);
        _mana.LateTick(delta);
    }

    public void Cleanup()
    {
        _stats.StatChanged -= PlayerStatManager_StatChanged;
    }

    private void PlayerStatManager_StatChanged(Stat stat, float value)
    {
        switch (stat)
        {
            case Stat.LightAttackDamage:
                _lightAttackData.Damage = value; break;

            case Stat.HeavyAttackDamage:
                _heavyAttackData.Damage = value; break;

            case Stat.AttackSpeed:
                _animatorManager.AttackSpeed = value; break;

            case Stat.MovementSpeedModifier:
                _animatorManager.MovementSpeed = value; break;

            case Stat.MovementSpeed:
                _locomotion.MovementSpeed = value; break;

            case Stat.HealthRegen:
                _health.RestorationModifier = value; break;

            case Stat.ManaRegen:
                _mana.RestorationModifier = value; break;

            case Stat.DamageReduction:
                _health.DamageReduction = value; break;

            case Stat.EvasionChance:
                _health.EvasionChance = value; break;
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