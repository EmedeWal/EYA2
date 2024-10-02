using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Stance : MonoBehaviour, IStanceDataProvider
{
    private Transform _transform;
    private float _delta;

    [Header("STANCE DATA")]
    [SerializeField] private StanceData _stanceData;

    [SerializeField] private List<PerkData> _passivePerks = new();
    [SerializeField] private List<PerkData> _ultimatePerks = new();

    private PlayerDataManager _playerDataManager;
    private VFX _stanceSmoke;

    private bool _active = false;

    private VFXManager _VFXManager;

    public StanceData StanceData => _stanceData;

    public void Init()
    {
        _transform = transform;

        _playerDataManager = GetComponent<PlayerDataManager>();

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

        foreach (var perk in _ultimatePerks)
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
    }

    public virtual void Exit()
    {
        _active = false;

        _VFXManager.RemoveVFX(_stanceSmoke, 1);

        foreach (var perk in _passivePerks)
        {
            perk.Deactivate();
        }
    }

    public virtual void CastUltimate()
    {
        foreach (var perk in _ultimatePerks)
        {
            perk.Activate();

            StartCoroutine(UltimateTickCoroutine(perk));
        }
    }

    public void AddPerk(PerkData perk)
    {
        if (perk.PerkType == PerkType.Passive)
        {
            AddPerk(_passivePerks, perk);

            if (_active) perk.Activate();
        }
        else
        {
            AddPerk(_ultimatePerks, perk);
        }
    }

    private void AddPerk(List<PerkData> perkList, PerkData perk)
    {
        perk.Init(perkList, gameObject); perkList.Add(perk);
    }

    private IEnumerator UltimateTickCoroutine(PerkData perk)
    {
        while (true)
        {
            yield return null;

            perk.Tick(_delta);
        }
    }
}
