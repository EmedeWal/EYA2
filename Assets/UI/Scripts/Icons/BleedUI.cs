using UnityEngine;
using TMPro;

public class BleedUI : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private TextMeshProUGUI _bleedText;
    [SerializeField] private GameObject _bleedIcon;
    private Bleed _bleed;

    // Variables
    private int _bleedStacks = 0;

    private void Awake()
    {
        _bleed = GetComponentInParent<Bleed>();
        _bleedIcon.SetActive(false);
    }

    private void OnEnable()
    {
        _bleed.BleedUpdate += BleedUI_BleedUpdate;
    }

    private void OnDisable()
    {
        _bleed.BleedUpdate -= BleedUI_BleedUpdate;
    }

    private void BleedUI_BleedUpdate(int modification)
    {
        ModifyBleedStacks(modification);

        if (modification < 0)
        {
            if (_bleedStacks == 0) _bleedIcon.SetActive(false);
        }
        else
        {
            _bleedIcon.SetActive(true);
        }
    }

    private void ModifyBleedStacks(int modification)
    {
        _bleedStacks += modification;
        _bleedText.text = _bleedStacks.ToString();
    }
}
