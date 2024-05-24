using UnityEngine;

public class AudioTime : MonoBehaviour
{
    [SerializeField] private float _offset;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _audioSource.time = _offset;
    }
}
