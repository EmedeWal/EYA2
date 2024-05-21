using System.Collections;
using UnityEngine;

public class VolumeManager : MonoBehaviour
{
    [Header("MUSIC MANAGEMENT")]
    [SerializeField] private float modifier = 10f;
    private AudioSource musicSource;
    private float defaultVolume;

    private void Awake()
    {
        musicSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        defaultVolume = musicSource.volume;
    }

    private void OnEnable()
    {
        Stance.UltimateStart += VolumeManager_UltimateStart;
    }

    private void OnDisable()
    {
        Stance.UltimateStart -= VolumeManager_UltimateStart;
    }

    private void VolumeManager_UltimateStart(float duration)
    {
        DecreaseVolume();

        Invoke(nameof(IncreaseVolume), duration);
    }

    private void DecreaseVolume()
    {
        musicSource.volume /= modifier;
    }

    private void IncreaseVolume()
    {
        StartCoroutine(IncreaseVolumeCoroutine());
    }

    private IEnumerator IncreaseVolumeCoroutine()
    {
        while (musicSource.volume < defaultVolume)
        {
            musicSource.volume += defaultVolume / modifier / 10;
            yield return new WaitForSeconds(defaultVolume / modifier);
        }
    }
}
