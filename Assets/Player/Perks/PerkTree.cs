using System.Collections.Generic;
using UnityEngine;

public class PerkTree : MonoBehaviour
{
    [Header("LINES")]
    [SerializeField] private Line _linePrefab;
    private Transform _lineParent;

    private List<Perk> _perks = new();
    private List<Line> _lines = new();

    private Color _purchasedColor;

    public void Init(Color purchasedColor)
    {
        _purchasedColor = purchasedColor;
        _lineParent = transform.GetChild(0);

        _perks.AddRange(GetComponentsInChildren<Perk>());
        foreach (var perk in _perks)
        {
            perk.Init(_purchasedColor);
            perk.OnPurchased += PerkTree_Purchased;
        }
    }

    public void UnlockPerksAtTier(int currentTier)
    {
        foreach (var perk in _perks)
        {
            int perkTier = perk.PerkData.Tier;

            if (perkTier <= currentTier && !perk.Unlocked)
            {
                perk.Unlock();
            }
        }
    }

    public void UpdateForeDrop(int currentSouls)
    {
        foreach (var perk in _perks)
        {
            if (perk.Locked || perk.Purchased) continue;

            if (perk.Unlocked)
            {
                int cost = perk.PerkData.Cost;
                if (currentSouls < cost)
                {
                    perk.SetForeDrop(true);
                }
                else
                {
                    perk.SetForeDrop(false);
                }
            }
        }
    }

    private void PerkTree_Purchased(Perk perk)
    {
        foreach (var nextPerk in perk.NextPerks)
        {
            DrawGreyLine(perk, nextPerk);
        }

        if (perk.PreviousPerk != null)
        {
            foreach (var line in _lines)
            {
                if (line.StartPerk == perk.PreviousPerk && line.EndPerk == perk)
                {
                    line.SetColor(_purchasedColor);
                }
            }
        }
    }

    private void DrawGreyLine(Perk perk, Perk nextPerk)
    {
        Line line = Instantiate(_linePrefab, transform);
        line.DrawLine(perk, nextPerk, _lineParent, Color.grey);
        _lines.Add(line);
    }
}
