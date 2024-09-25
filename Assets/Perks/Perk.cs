using System.Collections.Generic;
using UnityEngine;
using System;

public class Perk : MonoBehaviour, IClickable
{
    [Header("VISUALISATION")]
    [SerializeField] private GameObject _glowEffect;
    [SerializeField] private GameObject _foredrop;

    [Header("PERK DATA REFERENCE")]
    [SerializeField] private PerkData _perkData;

    private PerkScreen _perkScreen;

    [Header("PERK POSITION IN TREE")]
    public List<Perk> NextPerks = new();
    public Perk PreviousPerk;

    public PerkData PerkData => _perkData;

    public bool Locked { get; private set; } = false;
    public bool Unlocked { get; private set; } = false;
    public bool Purchased { get; private set; } = false;

    public event Action<Perk> OnPurchased;

    public virtual void Init()
    {
        _perkScreen = PerkScreen.Instance;
        _glowEffect.SetActive(false);
    }

    public virtual void OnEnter()
    {
        if (Unlocked)
        {
            _glowEffect.SetActive(true);
            _perkScreen.SetText(_perkData.Title, _perkData.Description);
        }
    }

    public virtual void OnExit()
    {
        if (Unlocked)
        {
            _glowEffect.SetActive(false);
            _perkScreen.SetText("", "");
        }
    }

    public virtual void OnClick()
    {
        if (Unlocked)
        {
            Purchase();
        }
    }

    public virtual void Unlock()
    {
        if (Locked) return;

        Unlocked = true;
        _foredrop.SetActive(false);
    }

    public virtual void Purchase()
    {
        if (Purchased || (PreviousPerk != null && !PreviousPerk.Purchased)) return;

        Purchased = true;
        OnPurchased?.Invoke(this);

        LockOtherBranch();
    }

    private void LockOtherBranch()
    {
        if (PreviousPerk != null)
        {
            foreach (var nextPerk in PreviousPerk.NextPerks)
            {
                if (nextPerk != this)
                {
                    nextPerk.LockBranch();
                }
            }
        }
    }

    public virtual void LockBranch()
    {
        Locked = true;
        Unlocked = false;
        _foredrop.SetActive(true);

        foreach (var nextPerk in NextPerks)
        {
            nextPerk.LockBranch();
        }
    }
}
