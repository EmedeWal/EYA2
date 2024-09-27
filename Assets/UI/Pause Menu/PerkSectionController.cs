using System.Collections.Generic;
using UnityEngine;

public class PerkSectionController : SectionControllerBase
{
    [Header("SECTION VISUALISATION")]
    [SerializeField] private StaticStanceIcon _staticStanceIcon;

    //[Header("PERK TIERS UNLOCKED")]
    //[SerializeField] private int _unlockedTiers = 0;

    private List<PerkTree> _perkTrees = new();

    public override void Init()
    {
        base.Init();

        _perkTrees.AddRange(_Holder.GetComponentsInChildren<PerkTree>());
        foreach (var perkTree in _perkTrees) perkTree.Init();
    }

    public override void Tick()
    {
        base.Tick();

        foreach (var perkTree in _perkTrees) perkTree.Tick();
    }

    public override void Added()
    {
        base.Added();

        _staticStanceIcon.Init(); IncrementTier();
     }

    public override void Select()
    {
        base.Select();

        _staticStanceIcon.SetGlow(true);
    }

    public override void Deselect()
    {
        base.Deselect();

        _staticStanceIcon.SetGlow(false);
    }

    public void IncrementTier()
    {
        foreach (var perkTree in _perkTrees)
        {
            perkTree.IncrementTier();
        }
    }
}
