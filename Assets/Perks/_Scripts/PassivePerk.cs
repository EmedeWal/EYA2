using System.Collections.Generic;
using UnityEngine;

public class PassivePerk : PerkData
{
    public override void Init(GameObject playerObject, List<PerkData> perks = null, Dictionary<Stat, float> statChanges = null)
    {
        for (int i = perks.Count - 1; i >= 0; i--)
        {
            PerkData perk = perks[i];
            if (perk.GetType() == GetType())
            {
                perk.Deactivate();
                perks.RemoveAt(i);
            }
        }

        base.Init(playerObject, perks, statChanges);
    }
}
