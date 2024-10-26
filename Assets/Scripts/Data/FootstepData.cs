using UnityEngine;

[CreateAssetMenu(fileName = "Footstep Data", menuName = "Scriptable Object/Data/Footstep Data")]
public class FootstepData : ScriptableObject
{
    [Header("AUDIO")]
    public AudioClip Clip;
    public float Volume;
    public float Offset;
}
