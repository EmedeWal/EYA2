using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Stance : MonoBehaviour, IStanceDataProvider
{
    private Transform _transform;
    private float _delta;

    private AudioSource _audioSource;
    private Mana _mana;

    [Header("STANCE DATA")]
    [SerializeField] private StanceData _stanceData;

    private List<PerkData> _passivePerks;
    private List<PerkData> _statPerks;
    private PerkData _ultimatePerk;

    private VFX _currentSmokeVFX;

    private bool _active = false;

    private AudioSystem _audioSystem;
    private VFXManager _VFXManager;

    public StanceData StanceData => _stanceData;

    public void Init(AudioSource audioSource)
    {
        _transform = transform;
        _audioSource = audioSource; 
        _mana = GetComponent<Mana>();

        _passivePerks = new List<PerkData>();
        _statPerks = new List<PerkData>();

        _audioSystem = AudioSystem.Instance;
        _VFXManager = VFXManager.Instance;
    }

    public void Tick(float delta)
    {
        _delta = delta;

        foreach (var perk in _passivePerks)
        {
            perk.Tick(_delta);
        }
    }

    public void CleanUp()
    {
        foreach (var perk in _passivePerks)
        {
            perk.Deactivate();
        }

        foreach (var perk in _statPerks)
        {
            perk.Deactivate();
        }
    }

    public virtual void Enter(bool sound)
    {
        _active = true;

        if (sound) _audioSystem.PlayAudio(_audioSource, _stanceData.SwapClip, _stanceData.SwapVolume, _stanceData.SwapOffset);

        _currentSmokeVFX = _VFXManager.AddVFX(_stanceData.SmokeVFX, false, 0f, _transform.position, _transform.rotation, _transform);

        foreach (var perk in _passivePerks)
        {
            perk.Activate();
        }

        foreach (var perk in _statPerks)
        {
            perk.Activate();
        }
    }

    public virtual void Exit()
    {
        _active = false;

        _VFXManager.RemoveVFX(_currentSmokeVFX, 1f);

        foreach (var perk in _passivePerks)
        {
            perk.Deactivate();
        }

        foreach (var perk in _statPerks)
        {
            perk.Deactivate();
        }
    }

    public virtual void CastUltimate()
    {
        if (_ultimatePerk != null && _mana.AtMaxValue())
        {
            _audioSystem.PlayExtraMusic(_stanceData.UltimateClip, _stanceData.UltimateVolume, _stanceData.UltimateOffset);
            _mana.ValueExhausted += Stance_ValueExhausted;
            StartCoroutine(UltimateTickCoroutine());
            _mana.RemoveConstantValue(10);
            _ultimatePerk.Activate();
        }
    }

    public void AddPerk(PerkData perk)
    {
        switch (perk.PerkType)
        {
            case PerkType.Stat:
                AddPerk(_statPerks, perk);
                break;

            case PerkType.Passive:
                AddPerk(_passivePerks, perk);
                break;

            case PerkType.Ultimate:

                if (_ultimatePerk != null)
                {
                    _mana.StopRemoveValueCoroutine();
                    _ultimatePerk.Deactivate();
                }

                _ultimatePerk = perk; 
                _ultimatePerk.Init(gameObject);
                break;
        }
    }

    private void AddPerk(List<PerkData> perkList, PerkData perk)
    {
        perk.Init(gameObject, perkList);
        if (_active) perk.Activate();
        perkList.Add(perk);
    }

    private void Stance_ValueExhausted(GameObject manaObject)
    {
        _mana.ValueExhausted -= Stance_ValueExhausted;
        _mana.StopRemoveValueCoroutine();
        _audioSystem.StopExtraMusic();
        _ultimatePerk.Deactivate();
    }

    private IEnumerator UltimateTickCoroutine()
    {
        while (true)
        {
            yield return null;
            _ultimatePerk.Tick(_delta);
        }
    }
}
