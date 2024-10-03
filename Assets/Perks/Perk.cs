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
    private AudioDataUI _audio;
    private Color _purchasedColor;

    public bool Locked { get; private set; } = false; 
    public bool Unlocked { get; private set; } = false; 
    public bool Purchased { get; private set; } = false;

    public event Action<Perk> PerkPurchased;

    public PerkData PerkData => _perkData;

    public virtual void Init(AudioDataUI audio, Color purchasedColor)
    {
        _audio = audio;
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
            _audio.System.PlaySilentClip(_audio.CommonSource, _audio.UncommonSource, _audio.SelectClip, _audio.SelectVolume, _audio.SelectOffset);
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
            OnPerkPurchased(this);
            _stanceIcon.Background.color = _purchasedColor;
            _audio.System.PlayAudioClip(_audio.UncommonSource, _audio.UnlockClip, _audio.UnlockVolume, _audio.UnlockOffset);
            PlayerStanceManager.Instance.AddPerk(_perkData, _perkData.StanceType);
            Souls.Instance.RemoveValue(_perkData.Cost);
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

    private void OnPerkPurchased(Perk perk)
    {
        PerkPurchased?.Invoke(perk);
    }

    private bool CanPurchase()
    {
        if (Purchased) return false;

        if ((PreviousPerk != null && !PreviousPerk.Purchased) || !Souls.Instance.CanAfford(_perkData.Cost))
        {
            _audio.System.PlayAudioClip(_audio.FailedSource, _audio.FailedClip, _audio.FailedVolume, _audio.FailedOffset, false); 
            return false;
        }
        return true;
    }
}
