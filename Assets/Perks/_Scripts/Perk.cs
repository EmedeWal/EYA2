using System.Collections.Generic;
using UnityEngine;
using System;

public class Perk : MonoBehaviour, IClickable
{
    [Header("PERK DATA REFERENCE")]
    [SerializeField] private PerkData _perkData;

    [Header("PERK POSITION IN TREE")]
    [SerializeField] private List<Perk> _nextPerks = new();
    [SerializeField] private Perk _otherBranchPerk;
    [SerializeField] private Perk _previousPerk;

    [Header("VISUALISATION")]
    [SerializeField] private Sprite _sprite;
    private Sprite _defaultSprite;

    private StanceIcon _stanceIcon;
    private GameObject _glowEffect;
    private GameObject _foredrop;
    private AudioDataUI _audio;
    private Color _purchasedColor;

    private PlayerStanceManager _playerStanceManager;
    private PerkScreen _perkScreen;
    private Souls _souls;

    public bool Locked { get; private set; } = false; 
    public bool Unlocked { get; private set; } = false; 
    public bool Purchased { get; private set; } = false;

    public event Action<Perk> PerkPurchased;

    public PerkData PerkData => _perkData;
    public List<Perk> NextPerks => _nextPerks;

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

        _playerStanceManager = PlayerStanceManager.Instance;
        _perkScreen = PerkScreen.Instance;
        _souls = Souls.Instance;
    }

    public virtual void OnEnter()
    {
        if (Unlocked)
        {
            _glowEffect.SetActive(true);
            _audio.System.PlaySilentClip(_audio.CommonSource, _audio.UncommonSource, _audio.SelectClip, _audio.SelectVolume, _audio.SelectOffset);
            _perkScreen.UpdatePerkScreen(_perkData.VideoClip, _perkData.Title, _perkData.Description, _perkData.Cost, _souls.CanAfford(_perkData.Cost), Purchased);
        }
    }

    public virtual void OnExit()
    {
        if (Unlocked)
        {
            _glowEffect.SetActive(false);
            _perkScreen.UpdatePerkScreen();
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
            OnPerkPurchased(this);
            _stanceIcon.Background.color = _purchasedColor;
            _audio.System.PlayAudio(_audio.UncommonSource, _audio.UnlockClip, _audio.UnlockVolume, _audio.UnlockOffset);

            if (_otherBranchPerk != null) 
            {
                _otherBranchPerk.LockBranch();
            }

            _playerStanceManager.AddPerk(_perkData, _perkData.StanceType);
            _souls.RemoveValue(_perkData.Cost);
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

    private void OnPerkPurchased(Perk perk)
    {
        PerkPurchased?.Invoke(perk);
    }

    private bool CanPurchase()
    {
        if (Purchased) return false;

        if ((_previousPerk != null && !_previousPerk.Purchased) || !Souls.Instance.CanAfford(_perkData.Cost))
        {
            _audio.System.PlayAudio(_audio.FailedSource, _audio.FailedClip, _audio.FailedVolume, _audio.FailedOffset, false); 
            return false;
        }
        return true;
    }
}
