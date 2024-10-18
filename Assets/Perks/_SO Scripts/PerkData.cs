using System.Collections.Generic;
using UnityEngine;

public class PerkData : ScriptableObject
{
    [Header("TYPE")]
    public StanceType StanceType;
    public PerkType PerkType;

    [Header("INFORMATION")]
    public string Title;
    public string Description;

    [Header("STATISTICS")]
    public int Tier;
    public int Cost;

    [Header("INHERITANCE PROPERTIES")]
    [SerializeField] protected PlayerStats _PlayerStats;
    protected StatTracker _StatTracker;
    protected AudioSystem _AudioSystem;
    protected VFXManager _VFXManager;
    protected GameObject _PlayerObject;
    protected Transform _PlayerTransform;
    protected PlayerAttackHandler _AttackHandler;
    protected PlayerLocomotion _Locomotion;
    protected PlayerLock _Lock;
    protected Health _Health;
    protected Mana _Mana;
    protected LayerMask _TargetLayer;

    public virtual void Init(GameObject playerObject, List<PerkData> perks = null, Dictionary<Stat, float> statChanges = null)
    {
        if (statChanges != null) _StatTracker = new StatTracker(statChanges, _PlayerStats);

        _AudioSystem = AudioSystem.Instance;
        _VFXManager = VFXManager.Instance;

        _PlayerObject = playerObject;
        _PlayerTransform = _PlayerObject.transform;

        _AttackHandler = _PlayerObject.GetComponent<PlayerAttackHandler>();
        _Locomotion = _PlayerObject.GetComponent<PlayerLocomotion>();
        _Lock = _PlayerObject.GetComponent<PlayerLock>();
        _Health = _PlayerObject.GetComponent<Health>();
        _Mana = _PlayerObject.GetComponent<Mana>();

        _TargetLayer = LayerMask.GetMask("DamageCollider");
    }

    public virtual void Activate()
    {

    }

    public virtual void Tick(float delta)
    {

    }

    public virtual void Deactivate()
    {

    }
}
