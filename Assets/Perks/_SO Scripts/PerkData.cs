using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PerkData", menuName = "Perks/PerkData")]
public class PerkData : ScriptableObject
{
    [Header("STANCE REFERENCE")]
    public StanceType StanceType;

    [Header("INFORMATION")]
    public string Title;
    public string Description;

    [Header("STATISTICS")]
    public int Tier;
    public int Cost;

    public virtual void Init(List<PerkData> perks, GameObject playerObject)
    {

    }

    public virtual void Activate()
    {

    }

    public virtual void Deactivate()
    {

    }
}
