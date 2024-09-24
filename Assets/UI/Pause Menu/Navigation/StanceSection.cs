using UnityEngine;

public class StanceSection : SectionControllerBase
{
    public override void Init()
    {
        base.Init();

        StancePerk[] stancePerks = GetComponentsInChildren<StancePerk>();
        foreach (var stancePerk in stancePerks)
        {
            Debug.Log("Init");
            stancePerk.Init();
        }
    }
}
