using UnityEngine;
using System;

public class PlayerDataManager : MonoBehaviour
{
    public ReferenceStruct ReferenceStruct;
    public LocomotionStruct LocomotionStruct;
    public UltimateStruct UltimateStruct;
    public LockOnStruct LockOnStruct;
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
