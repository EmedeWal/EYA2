using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PerkData", menuName = "Perks/PerkData")]
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
    protected GameObject _PlayerObject;
    protected Transform _PlayerTransform;

    public virtual void Init(List<PerkData> perks, GameObject playerObject)
    {
        _PlayerObject = playerObject;
        _PlayerTransform = _PlayerObject.transform;
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
