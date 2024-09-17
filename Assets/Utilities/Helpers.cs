using UnityEditor;
using UnityEngine;

public static class Helpers
{
    public static Collider[] CastHitbox(Vector3 attackPoint, Vector3 attackSize, Quaternion rotation, int layerMask)
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
}
