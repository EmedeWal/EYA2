using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    public List<AudioSource> _AudioSources { get { return _audioSources; } }

    [Header("AUDIO SOURCES")]
    [SerializeField] private List<AudioSource> _audioSources = new();

    public void PlayAudioClip(AudioSource audioSource, AudioClip audioClip, float offset)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }
}
