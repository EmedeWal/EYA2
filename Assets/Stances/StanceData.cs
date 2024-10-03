using UnityEngine;

[CreateAssetMenu(fileName = "New Stance Data", menuName = "Scriptable Object/Data/Stance Data")]
public class StanceData : ScriptableObject
{
    [Header("STANCETYPE")]
    public StanceType StanceType;

    [Header("VISUALIZATION")]
    public Sprite IconSprite;
    public Color Color;

    [Header("VFX")]
    public VFX Smoke;

    [Header("SWAP AUDIO")]
    public AudioClip SwapClip;
    public float SwapVolume;
    public float SwapOffset;

    [Header("ULTIMATE AUDIO")]
    public AudioClip UltimateClip;
    public float UltimateVolume;
    public float UltimateOffset;
}
