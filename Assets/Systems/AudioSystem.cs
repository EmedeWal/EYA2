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

        MusicSource =GetComponent<AudioSource>();
    }
    #endregion

    public AudioSource MusicSource { get; private set; }
    public float VolumeModifier = 0.5f;

    public void PlayAudioClip(AudioSource source, AudioClip clip, float offset = 0, float volume = 0, bool overrideAudio = true)
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

        if (volume != 0)
        {
            source.volume = volume;
        }

        if (offset != 0)
        {
            source.time = offset;
        }

        source.Play();
    }
}
