using UnityEngine;

[CreateAssetMenu(fileName = "Explosion Data", menuName = "Scriptable Object/Data/Explosion Data")]
public class ExplosionData : ScriptableObject
{
    [Header("REFERENCES")]
    public VFX VFX;

    [Header("VAIRABLES")]
    public float Radius;
    public float Damage;
}
