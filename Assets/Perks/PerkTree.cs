using System.Collections.Generic;
using UnityEngine;

public class PerkTree : MonoBehaviour
{
    [Header("LINES")]
    [SerializeField] private Transform _lineParent;
    [SerializeField] private Line _linePrefab;

    [Header("UNLOCKED TIERS")]
    [SerializeField] private int _unlockedTiers;

    public StanceData StanceData;

    private List<Perk> _perks = new();
    private List<Line> _lines = new();

    public void Init()
    {
        _perks.AddRange(GetComponentsInChildren<Perk>());
        foreach (var perk in _perks)
        {
            perk.Init();
            perk.OnPurchased += HandlePerkPurchased;
        }

        Souls.Instance.CurrentValueUpdated += PerkTree_CurrentValueUpdated;
    }

    public void IncrementTier()
    {
        if (_unlockedTiers >= 5) return;

        _unlockedTiers++;

        foreach (var perk in _perks)
        {
            if (perk.PerkData.Tier <= _unlockedTiers && !perk.Unlocked)
            {
                perk.Unlock();
            }
        }
    }

    private void HandlePerkPurchased(Perk perk)
    {
        foreach (var nextPerk in perk.NextPerks)
        {
            Line line = Instantiate(_linePrefab, transform);
            line.DrawLine(perk, nextPerk, _lineParent, Color.grey);
            _lines.Add(line);
        }

        if (perk.PreviousPerk != null)
        {
            foreach (var line in _lines)
            {
                if (line.StartPerk == perk.PreviousPerk && line.EndPerk == perk)
                {
                    line.SetColor(StanceData.Color);
                }
            }
        }
    }

    private void PerkTree_CurrentValueUpdated(int currentValue)
    {
        foreach (var perk in _perks)
        {
            if (!perk.Unlocked) return;

            int cost = perk.PerkData.Cost;

            if (currentValue < cost)
            {
                perk.SetForedrop(true);
            }
            else
            {
                perk.SetForedrop(false);
            }
        }
    }
}
