using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class Stance : MonoBehaviour, IStanceDataProvider
{
    private Transform _transform;
    private float _delta;

    private Mana _mana;

    [Header("STANCE DATA")]
    [SerializeField] private StanceData _stanceData;

    [SerializeField] private List<PerkData> _passivePerks;
    [SerializeField] private List<PerkData> _statPerks;
    [SerializeField] private PerkData _ultimatePerk;

    private VFX _stanceSmoke;

    private bool _active = false;

    private VFXManager _VFXManager;

    public StanceData StanceData => _stanceData;

    public void Init()
    {
        _transform = transform;

        _mana = GetComponent<Mana>();

        _passivePerks = new List<PerkData>();
        _statPerks = new List<PerkData>();

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

    public virtual void Enter()
    {
        _active = true;

        _stanceSmoke = Instantiate(_stanceData.Smoke, _transform);
        _VFXManager.AddVFX(_stanceSmoke, _transform);

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

        _VFXManager.RemoveVFX(_stanceSmoke, 1);

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
