using System.Collections.Generic;
using UnityEngine;

public class PerkSectionController : SectionControllerBase
{
    [Header("STANCE DATA REFERENCE")]
    [SerializeField] private StanceData _stanceData;

    [Header("UNLOCKED TIERS")]
    [SerializeField] private int _unlockedTiers = 0;
    private int _currentSouls;

    private List<PerkTree> _perkTrees = new();

    public override void Init()
    {
        base.Init();

        _perkTrees.AddRange(_Holder.GetComponentsInChildren<PerkTree>());
        foreach (var perkTree in _perkTrees) perkTree.Init(_stanceData.Color);
        Souls.Instance.CurrentValueUpdated += PerkSectionController_CurrentValueUpdated;

        IncrementTier();
    }

    public void IncrementTier()
    {
        if (_unlockedTiers >= 5) return;

        _unlockedTiers++;

        foreach (var perkTree in _perkTrees)
        {
            perkTree.UnlockPerksAtTier(_unlockedTiers);
        }

        UpdateForeDrops();
    }

    private void PerkSectionController_CurrentValueUpdated(int currentValue)
    {
        _currentSouls = currentValue;
        UpdateForeDrops();
    }

    private void UpdateForeDrops()
    {
        foreach (var perktree in _perkTrees)
        {
            perktree.UpdateForeDrop(_currentSouls);
        }
    }
}
