using UnityEngine;

[CreateAssetMenu(fileName = "UI Audio Data", menuName = "Scriptable Object/Data/Clickable Audio Data")]
public class AudioDataUI : ScriptableObject
{
    [HideInInspector] public AudioSource UncommonSource;
    [HideInInspector] public AudioSource CommonSource;
    [HideInInspector] public AudioSource FailedSource;
    [HideInInspector] public AudioSystem System;

    [Header("PAUSE AUDIO")]
    public AudioClip PauseClip;
    public float PauseVolume;
    public float PauseOffset;

    [Header("RESUME AUDIO")]
    public AudioClip ResumeClip;
    public float ResumeVolume;
    public float ResumeOffset;

    [Header("SWAP AUDIO")]
    public AudioClip SwapClip;
    public float SwapVolume;
    public float SwapOffset;

    [Header("SELECT AUDIO")]
    public AudioClip SelectClip;
    public float SelectVolume;
    public float SelectOffset;

    [Header("UNLOCK AUDIO")]
    public AudioClip UnlockClip;
    public float UnlockVolume;
    public float UnlockOffset;

    [Header("FAILED AUDIO")]
    public AudioClip FailedClip;
    public float FailedVolume;
    public float FailedOffset;
}
