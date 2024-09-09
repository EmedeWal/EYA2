using UnityEngine;
using System;

public class PlayerDataManager : MonoBehaviour
{
    #region Movement
    [SerializeField] private MovementData _movementData;

    public void SetDirection(Vector2 newDirection)
    {
        _movementData.Direction = newDirection;
    }

    public Vector2 GetDirection()
    {
        return _movementData.Direction;
    }

    public void SetMovementModifier(float newModifier)
    {
        _movementData.MovementModifier = newModifier;
    }

    public float GetMovementModifier()
    {
        return _movementData.MovementModifier;
    }
    #endregion

    #region Dashing

    [SerializeField] DashData _dashData;

    public void SetDashModifier(float cooldownModifier)
    {
        _dashData.DashModifier = cooldownModifier;
    }

    public float GetDashModifier()
    {
        return _dashData.DashModifier;
    }

    #endregion

    #region Attacking
    [SerializeField] private AttackData _attackData;

    public void SetAttackModifier(float newModifier)
    {
        _attackData.AttackModifier = newModifier;
    }

    public float GetAttackModifier()
    {
        return _attackData.AttackModifier;
    }
    #endregion

    #region Stances

    [SerializeField] private StanceData _stanceData;

    public void SetLifeSteal(float newLifeSteal)
    {
        _stanceData.LifeSteal = newLifeSteal;
    }

    public float GetLifeSteal()
    {
        return _stanceData.LifeSteal;
    }

    public void SetDamageReduction(float newModifier)
    {
        _stanceData.DamageReduction = newModifier;
    }

    public float GetDamageReduction()
    {
        return _stanceData.DamageReduction;
    }

    #endregion

    #region Ultimate

    private UltimateData _ultimateData;

    public void SetUltimateActivate(bool active)
    {
        _ultimateData.IsUltimateActive = active;
    }

    public bool GetUltimateActivate()
    {
        return _ultimateData.IsUltimateActive;
    }

    #endregion

    public LockOnData LockOnData { get; private set; }

    #region Visual Effects

    [SerializeField] private VFXData _VFXData;

    public Transform GetVFXOrigin()
    {
        return _VFXData.VFXOrigin;
    }

    #endregion
}

[Serializable]
public struct MovementData
{
    [HideInInspector] public Vector2 Direction;
    public float MovementModifier;
}

[Serializable]
public struct DashData
{
    public float DashModifier;
}

[Serializable]
public struct AttackData
{
    public float AttackModifier;
}

[Serializable]
public struct StanceData
{
    public float LifeSteal;

    public float DamageReduction;
}

public struct UltimateData
{
    public bool IsUltimateActive;
}

[Serializable]
public struct VFXData
{
    public Transform VFXOrigin;
}

public struct LockOnData
{
    public Transform LockOnTarget;
    public bool LockedOn;
}
