using UnityEngine;

public class PerkSectionController : SectionControllerBase
{
    public override void Init()
    {
        base.Init();

        Perk[] perks = _Holder.GetComponentsInChildren<Perk>();
        foreach (var perk in perks) perk.Init();
    }
}
