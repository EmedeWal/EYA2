using UnityEngine;

[CreateAssetMenu(fileName = "New Stance Data", menuName = "Stance Data")]
public class StanceData : ScriptableObject
{
    [Header("STANCETYPE")]
    public StanceType StanceType;

    [Header("GENERAL")]
    public float UltimateDuration = 10;

    [Header("VISUALIZATION")]
    public GameObject UltimateGFX;
    public GameObject SwapVFX;
    public Sprite IconSprite;
    public Color Color;

    [Header("AUDIO")]
    public AudioClip UltimateClip;
    public float AudioVolume;
    public float AudioOffset;
}
