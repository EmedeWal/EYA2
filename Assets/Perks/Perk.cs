using System.Collections.Generic;
using UnityEngine;
using System;

public class Perk : MonoBehaviour, IClickable
{
    [Header("PERK DATA REFERENCE")]
    [SerializeField] private PerkData _perkData;

    [Header("PERK POSITION IN TREE")]
    public List<Perk> NextPerks = new();
    public Perk PreviousPerk;

    [Header("VISUALISATION")]
    [SerializeField] private Sprite _sprite;
    private Sprite _defaultSprite;

    private StanceIcon _stanceIcon;
    private GameObject _glowEffect;
    private GameObject _foredrop;
    private Color _purchasedColor;

    public bool Locked { get; private set; } = false; 
    public bool Unlocked { get; private set; } = false; 
    public bool Purchased { get; private set; } = false;

    public event Action<Perk> OnPurchased;

    public PerkData PerkData => _perkData;

    public virtual void Init(Color purchasedColor)
    {
        _purchasedColor = purchasedColor;
        _stanceIcon = GetComponent<StanceIcon>();
        _glowEffect = transform.GetChild(0).gameObject;
        _foredrop = transform.GetChild(transform.childCount - 1).gameObject;

        _stanceIcon.Background.color = Color.clear;
        _defaultSprite = _stanceIcon.Icon.sprite;
        _glowEffect.SetActive(false);
    }

    public virtual void OnEnter()
    {
        if (Unlocked)
        {
            _glowEffect.SetActive(true);
            PerkScreen.Instance.UpdatePerkScreen(_perkData.Title, _perkData.Description, _perkData.Cost, Souls.Instance.CanAfford(_perkData.Cost), Purchased);
        }
    }

    public virtual void OnExit()
    {
        if (Unlocked)
        {
            _glowEffect.SetActive(false);
            PerkScreen.Instance.UpdatePerkScreen();
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
        _stanceIcon.Icon.sprite = _sprite;
    }

    public virtual void Purchase()
    {
        if (CanPurchase())
        {
            Purchased = true;
            LockOtherBranch();
            OnPurchased?.Invoke(this);
            _stanceIcon.Background.color = _purchasedColor;

            Souls.Instance.RemoveValue(_perkData.Cost);
            PlayerStanceManager.Instance.AddPerk(_perkData, _perkData.StanceType);
        }
    }

    public virtual void LockBranch()
    {
        Locked = true;
        Unlocked = false;
        SetForeDrop(true);
        _stanceIcon.Icon.sprite = _defaultSprite;

        foreach (var nextPerk in NextPerks)
        {
            nextPerk.LockBranch();
        }
    }

    public void SetForeDrop(bool active)
    {
        _foredrop.SetActive(active);
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

    private bool CanPurchase()
    {
        if (Purchased || (PreviousPerk != null && !PreviousPerk.Purchased) || !Souls.Instance.CanAfford(_perkData.Cost)) return false;
        return true;
    }
}
