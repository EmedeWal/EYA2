using UnityEngine;

public abstract class Stance : MonoBehaviour
{
    // References
    protected PlayerDataManager DataManager;
    protected PlayerHealth Health;

    [Header("STANCE RELATED")]
    [SerializeField] private StanceType _stanceType;
    [SerializeField] protected float UltimateDuration = 10f;

    [Header("VISUALS")]
    [SerializeField] private GameObject _ultimateGFX;
    [SerializeField] private GameObject _swapVFX;
    [SerializeField] private Transform _origin;
    [SerializeField] private Renderer _swordRender;
    [SerializeField] private Color _stanceColor;
    private GameObject _currentUltimateGFX;

    [Header("AUDIO")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _ultimateClip;
    [SerializeField] private float _audioVolume = 0.1f;
    [SerializeField] private float _audioOffset = 0;

    public delegate void Delegate_UltimateStart(float duration);
    public static event Delegate_UltimateStart UltimateStart;

    public delegate void Delegate_UpdateStance(StanceType stanceType);
    public static event Delegate_UpdateStance UpdateStance;

    protected virtual void Awake()
    {
        DataManager = GetComponent<PlayerDataManager>();
        Health = GetComponent<PlayerHealth>();
    }

    protected virtual void ManageStanceSwap()
    {
        ManageColors();

        UpdateStance?.Invoke(_stanceType);
    }

    private void ManageColors()
    {
        Instantiate(_swapVFX, _origin); 

        _swordRender.material.color = _stanceColor;
    }

    protected void ActivateUltimate()
    {
        DataManager.SetUltimateActivate(true);

        _currentUltimateGFX = Instantiate(_ultimateGFX, DataManager.GetVFXOrigin());

        UltimateStart?.Invoke(UltimateDuration);

        PlayAudio();
    }

    protected void DeactivateUltimate()
    {
        DataManager.SetUltimateActivate(false);

        Destroy(_currentUltimateGFX);

        _audioSource.Stop();
    }

    private void PlayAudio()
    {
        _audioSource.volume = _audioVolume;
        _audioSource.clip = _ultimateClip;
        _audioSource.time = _audioOffset;
        _audioSource.Play();
    }
}
