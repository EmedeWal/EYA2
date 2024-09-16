using UnityEngine;

public static class Helpers
{
    public static Collider[] CastHitbox(Vector3 attackPoint, Vector3 attackSize, Quaternion rotation, int layerMask)
    {
        return Physics.OverlapBox(attackPoint, attackSize * 0.5f, rotation, layerMask);
    }

    public static int GetIndexIncrement(int index, int length)
    {
        index++;

        if (index >= length)
        {
            return 0;
        }

        return index;
    }
}
