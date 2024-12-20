using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class PerkScreen : SingletonBase
{
    #region Singleton
    public static PerkScreen Instance { get; private set; }

    public override void SingletonSetup()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    [Header("TEXT REFERENCES")]
    [SerializeField] private TextMeshProUGUI[] _texts;

    [Header("ICON REFERENCE")]
    [SerializeField] private Sprite _iconSprite;
    [SerializeField] private Image _iconImage;

    [Header("VISUALISATION")]
    [SerializeField] private Color _canAfford;
    [SerializeField] private Color _cannotAfford;

    [Header("VIDEO PLAYER")]
    [SerializeField] private GameObject _rawImageObject;
    private VideoPlayer _videoPlayer;

    [Header("HORIZONTAL LAYOUT GROUP")]
    [SerializeField] private GameObject _horizontalLayoutGroupObject;

    public void Init()
    {
        _videoPlayer = GetComponent<VideoPlayer>();
        _iconImage.sprite = _iconSprite;
    }

    public void UpdatePerkScreen(VideoClip videoClip = null, string title = "", string description = "", int cost = 0, bool canAfford = false, bool purchased = true)
    {
        Color color;

        if (canAfford)
        {
            color = _canAfford;
        }
        else
        {
            color = _cannotAfford;
        }

        if (purchased)
        {
            _horizontalLayoutGroupObject.SetActive(false);
        }
        else
        {
            _horizontalLayoutGroupObject.SetActive(true);
        }

        _videoPlayer.clip = videoClip;

        if (videoClip != null)
        {
            _rawImageObject.SetActive(true);
            _videoPlayer.Play();
        }
        else
        {
            _rawImageObject.SetActive(false);
            _videoPlayer.Stop();
        }

        _texts[0].text = title;
        _texts[2].text = description;

        _texts[1].text = cost.ToString();
        _texts[1].color = color;
    }
}