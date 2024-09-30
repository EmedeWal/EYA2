using UnityEngine;

public static class Helpers
{
    public static Collider[] CastHitBox(Vector3 attackPoint, Vector3 attackSize, Quaternion rotation, int layerMask)
    {
        return Physics.OverlapBox(attackPoint, attackSize * 0.5f, rotation, layerMask);
    }

    public static int GetIndexInBounds(int index, int increment, int length)
    {
        int lastPosition = length - 1;

        index += increment;

        if (index < 0)
        {
            index = lastPosition;
        }
        else if (index > lastPosition)
        {
            index = 0;
        }

        return index;
    }

    public static string FormatStatName(Stat stat)
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

    public static string GetStatLineEnd(Stat stat)
    {
        string lineEnd = "";

        if (IsPercentageStat(stat))
        {
            lineEnd = "%";
        }
        else if (IsMultiplierStat(stat))
        {
            lineEnd = "x";
        }
        else if (IsRegenStat(stat))
        {
            lineEnd = "/s";
        }

        return lineEnd;

        bool IsPercentageStat(Stat stat)
        {
            return stat == Stat.DamageReduction ||
                   stat == Stat.CriticalChance ||
                   stat == Stat.EvasionChance;
        }

        bool IsMultiplierStat(Stat stat)
        {
            return stat == Stat.CriticalMultiplier ||
                   stat == Stat.StaggerMultiplier;
        }

        bool IsRegenStat(Stat stat)
        {
            return stat == Stat.HealthRegen ||
                   stat == Stat.ManaRegen;
        }
    }

    public static bool GetChanceRoll(float chance)
    {
        int random = Random.Range(0, 100);

        if (chance > random)
        {
            return true;
        }
        return false;
    }
}
