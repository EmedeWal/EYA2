using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public void Initialize()
    {
        LocomotionData.SpeedModifier = 1;
        AttackData.AttackModifier = 1;
        StanceData.DamageReduction = 0;
        StanceData.LifeSteal = 0;
    }

    public LocomotionData LocomotionData;
    public LockOnData LockOnData;
    public AttackData AttackData;
    public StanceData StanceData;
    public UltimateData UltimateData;
}

public struct LocomotionData
{
    public float SpeedModifier;
    public bool ForceAdded;
}

public struct AttackData
{
    public float AttackModifier;
}

public struct StanceData
{
    public float LifeSteal;
    public float DamageReduction;
}

public struct UltimateData
{
    public bool IsUltimateActive;
}

public struct LockOnData
{
    public Transform LockOnTarget;
    public bool LockedOn;
}
