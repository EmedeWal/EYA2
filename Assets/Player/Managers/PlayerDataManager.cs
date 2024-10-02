using UnityEngine;
using System;

public class PlayerDataManager : MonoBehaviour
{
    public ReferenceStruct ReferenceStruct;
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
