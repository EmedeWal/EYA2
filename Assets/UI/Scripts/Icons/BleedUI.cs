using UnityEngine;
using TMPro;

public class BleedUI : MonoBehaviour
{
    [Header("BLEED SYSTEM")]
    [SerializeField] private TextMeshProUGUI _bleedText;
    [SerializeField] private GameObject _bleedIcon;
    private int _bleedStacks = 0;

    public void AddBleed()
    {
        ModifyBleedStacks(1);

        _bleedIcon.SetActive(true);
    }

    public void RemoveBleed()
    {
        ModifyBleedStacks(-1);

        if (_bleedStacks == 0)
        {
            _bleedIcon.SetActive(false);
        }
    }

    private void ModifyBleedStacks(int modification)
    {
        _bleedStacks += modification;
        _bleedText.text = _bleedStacks.ToString();
    }
}
