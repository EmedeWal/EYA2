using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour, IClickable
{
    public PerkData PerkData;
    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
        // Set initial visuals based on PerkData
    }

    public void OnEnter()
    {
        Debug.Log("OnHover");
        // Highlight or change visuals
        // Show details in the SkillDetailPanel
    }

    public void OnExit()
    {
        Debug.Log("OnExit");
        // Reset visuals
    }

    public void OnClick()
    {
        Debug.Log("OnClick");
        // Handle perk activation or detailed display
        // Maybe trigger a function in the SkillDetailPanel to update with this perk's info
    }
}
