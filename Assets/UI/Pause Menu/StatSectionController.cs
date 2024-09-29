using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatSectionController : SectionControllerBase
{
    [Header("PLAYER STAT REFERENCE")]
    [SerializeField] private PlayerStats _playerStats;

    [Header("TEXT OBJECTS")]
    public TextMeshProUGUI[] _texts;

    private Dictionary<Stat, int> _statToTextIndex;

    public override void Init()
    {
        base.Init();

        _statToTextIndex = new Dictionary<Stat, int>
        {
            { Stat.CurrentHealth, 0 },      { Stat.MovementSpeed, 6 },
            { Stat.CurrentMana, 1 },        { Stat.StaggerMultiplier, 7 },
            { Stat.HealthRegen, 2 },        { Stat.LightAttackDamage, 8 },
            { Stat.ManaRegen, 3 },          { Stat.HeavyAttackDamage, 9 },
            { Stat.EvasionChance, 4 },      { Stat.CriticalChance, 10 },
            { Stat.DamageReduction, 5 },    { Stat.CriticalMultiplier, 11 },
        };

        _playerStats.StatChanged += OnStatChanged;

        foreach (var statTextPair in _statToTextIndex)
        {
            Stat stat = statTextPair.Key;
            int textIndex = statTextPair.Value;
            float statValue = _playerStats.GetStat(stat);

            SetStatText(_texts[textIndex], stat, statValue);
        }
    }

    private void OnStatChanged(Stat stat, float value)
    {
        if (_statToTextIndex.TryGetValue(stat, out int textIndex))
        {
            SetStatText(_texts[textIndex], stat, value);
        }
    }

    private void SetStatText(TextMeshProUGUI text, Stat stat, float value)
    {
        string textString = $"{FormatStatName(stat)} = {value}";

        if (IsPercentageStat(stat))
        {
            textString += "%";
        }
        else if (IsMultiplierStat(stat))
        {
            textString += "x";
        }
        else if (IsRegenStat(stat))
        {
            textString += "/s";
        }

        text.text = textString;
    }

    private string FormatStatName(Stat stat)
    {
        string statName = stat.ToString();

        System.Text.StringBuilder formattedName = new System.Text.StringBuilder();

        for (int i = 0; i < statName.Length; i++)
        {
            if (char.IsUpper(statName[i]) && i > 0)
            {
                formattedName.Append(' ');
            }

            formattedName.Append(statName[i]);
        }

        return formattedName.ToString();
    }

    private bool IsPercentageStat(Stat stat)
    {
        return stat == Stat.DamageReduction ||
               stat == Stat.CriticalChance ||
               stat == Stat.EvasionChance;
    }

    private bool IsMultiplierStat(Stat stat)
    {
        return stat == Stat.CriticalMultiplier ||
               stat == Stat.StaggerMultiplier;
    }

    private bool IsRegenStat(Stat stat)
    {
        return stat == Stat.HealthRegen ||
               stat == Stat.ManaRegen;
    }
}
