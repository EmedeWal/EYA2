using UnityEngine;
using System;

public class PlayerDataManager : MonoBehaviour
{
    public void Init()
    {
        LocomotionStruct.SpeedModifier = 1;
        AttackStruct.AttackModifier = 1;
        StanceStruct.DamageReduction = 0;
        StanceStruct.LifeSteal = 0;
    }

    public ReferenceStruct ReferenceStruct;

    public LocomotionStruct LocomotionStruct;
    public UltimateStruct UltimateStruct;
    public LockOnStruct LockOnStruct;
    public AttackStruct AttackStruct;
    public StanceStruct StanceStruct;
}

[Serializable]
public struct ReferenceStruct
{
    public AudioSource FootstepSource;
    public AudioSource GeneralSource;
    public AudioSource AttackSource;
    public Transform Center;
    public Renderer Sword;
}

public struct LocomotionStruct
{
    public float SpeedModifier;
    public bool ForceAdded;
}

public struct UltimateStruct
{
    public bool IsUltimateActive;
}

public struct LockOnStruct
{
    public Transform LockOnTargetTransform;
    public bool LockedOn;
}

public struct AttackStruct
{
    public float AttackModifier;
}

public struct StanceStruct
{
    public float LifeSteal;
    public float DamageReduction;
}
