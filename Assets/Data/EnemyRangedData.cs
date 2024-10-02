using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Ranged Data", menuName = "Scriptable Object/Data/Enemy Ranged Data", order = 51)]
public class EnemyRangedData : ScriptableObject
{
    [Header("PROJECTILE")]
    public GameObject ProjectilePrefab;

    [Header("TIMING")]
    public float ChargeDuration;
    public float FireDuration;
    public float FireCooldown;

    [Header("LETHALITY")]
    public float FireRange;

    [Header("ANIMATION")]
    public string AnimationParameter;

    [Header("AUDIO")]
    public AudioClip FireClip;
}
