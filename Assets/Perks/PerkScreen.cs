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

    private TextMeshProUGUI[] _texts;

    public void Init()
    {
        _texts = GetComponentsInChildren<TextMeshProUGUI>();
    }

    public void SetText(string title, string description)
    {
        _texts[0].text = title;
        _texts[1].text = description;
    }
}
