using UnityEngine;

public class FootstepHandler : MonoBehaviour
{
    [Header("FOOTSTEP DATA REFERENCE")]
    [SerializeField] private FootstepData _footstepData;

    [Header("FOOTSTEP SOURCE")]
    [SerializeField] private AudioSource _audioSource;

    private IMovingProvider _movingProvider;
    private AudioSystem _audioSystem;

    public void Init()
    {
        _movingProvider = GetComponent<IMovingProvider>();
        _audioSystem = AudioSystem.Instance;
    }

    public void Footsteps()
    {
        if (_movingProvider.Moving)
        {
            _audioSystem.PlayAudio(_audioSource, _footstepData.Clip, _footstepData.Volume, _footstepData.Offset);
        }
    }
}
