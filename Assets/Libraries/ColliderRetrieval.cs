using UnityEngine;

public static class ColliderRetrieval
{
    public static Collider[] CastHitbox(Vector3 attackPoint, Vector3 attackSize, Quaternion rotation, int layerMask)
    {
        return Physics.OverlapBox(attackPoint, attackSize * 0.5f, rotation, layerMask);
    }
}
