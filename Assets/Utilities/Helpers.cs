using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Helpers
{
    public static Collider[] CastHitBox(Vector3 attackPoint, Vector3 attackSize, Quaternion rotation, int layerMask)
    {
        return Physics.OverlapBox(attackPoint, attackSize * 0.5f, rotation, layerMask);
    }

    public static Color GetTransparentColor()
    {
        float r, g, b, a;

        r = 0;
        g = 0;
        b = 0;
        a = 0;

        return new Color(r, g, b, a);
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

    public static void SortByStanceType<T>(List<T> list) where T : class
    {
        var stanceItems = list.OfType<IStanceDataProvider>().ToList();
        stanceItems.Sort((a, b) => a.StanceData?.StanceType.CompareTo(b.StanceData?.StanceType) ?? 0);

        int stanceIndex = 0;
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] is IStanceDataProvider)
            {
                list[i] = stanceItems[stanceIndex] as T;
                stanceIndex++;
            }
        }
    }

}
