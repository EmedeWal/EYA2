using UnityEngine;

public abstract class Stance : MonoBehaviour
{
    // References
    protected PlayerDataManager _DataManager;
    protected Health _Health;

    [Header("REFERENCES")]
    [SerializeField] private Transform _center;

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
        _DataManager = GetComponent<PlayerDataManager>();
        _Health = GetComponent<Health>();
    }

    protected virtual void ManageStanceSwap()
    {
        ManageColors();
        OnUpdateStance();
    }

    private void ManageColors()
    {
        Instantiate(_swapVFX, _origin); 
        _swordRender.material.color = _stanceColor;
    }

    protected void ActivateUltimate()
    {
        _DataManager.UltimateData.IsUltimateActive = true;
        _currentUltimateGFX = Instantiate(_ultimateGFX, _center);
        OnUltimateStart();
        PlayAudio();
    }

    protected void DeactivateUltimate()
    {
        _DataManager.UltimateData.IsUltimateActive = false;
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

    private void OnUltimateStart()
    {
        UltimateStart?.Invoke(UltimateDuration);
    }

    private void OnUpdateStance()
    {
        UpdateStance?.Invoke(_stanceType);
    }
}
