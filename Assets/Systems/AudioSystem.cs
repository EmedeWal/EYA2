using UnityEngine;

public class AudioSystem : SingletonBase
{
    #region Singleton
    public static AudioSystem Instance;

    public override void SingletonSetup()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    [Header("AUDIO SOURCES")]
    [SerializeField] private AudioSource _defaultMusicSource;
    [SerializeField] private AudioSource _extraMusicSource;
    private float _defaultVolume;

    public float VolumeModifier = 0.5f;

    public void Init()
    {
        _defaultVolume = _defaultMusicSource.volume;
        _defaultMusicSource.volume = _defaultVolume * VolumeModifier;
    }

    public void SetVolumeModifier(float modifier)
    {
        VolumeModifier = modifier;
        _defaultMusicSource.volume = _defaultVolume * VolumeModifier;
    }

    public void PlayAudioClip(AudioSource source, AudioClip clip, float volume, float offset = 0, bool overrideAudio = true)
    {
        volume *= VolumeModifier;

        if (source.isPlaying)
        {
            if (overrideAudio)
            {
                source.Stop();
            }
            else
            {
                return;
            }
        }

        source.clip = clip;
        source.time = offset;
        source.volume = volume;

        source.Play();
    }

    public void PlaySilentClip(AudioSource mainSource, AudioSource secondSource, AudioClip clip, float volume, float offset = 0)
    {
        volume *= VolumeModifier;

        if (secondSource.isPlaying)
        {
            return;
        }

        mainSource.Stop();

        mainSource.clip = clip;
        mainSource.time = offset;
        mainSource.volume = volume;

        mainSource.Play();
    }

    public void PlayExtraMusic(AudioClip clip, float volume, float offset = 0)
    {
        _defaultMusicSource.volume = _defaultVolume * VolumeModifier * 0.1f;

        _extraMusicSource.clip = clip;
        _extraMusicSource.time = offset;
        _extraMusicSource.volume = volume * VolumeModifier;
        _extraMusicSource.Play();
    }

    public void StopExtraMusic()
    {
        _extraMusicSource.Stop();
        _defaultMusicSource.volume = _defaultVolume * VolumeModifier;
    }
}
