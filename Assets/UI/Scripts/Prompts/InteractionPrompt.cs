using UnityEngine;

public class InteractionPrompt : MonoBehaviour
{
    [SerializeField] private GameObject _prompt;

    private void Start()
    {
        ManagePrompt(false);
    }

    private void OnEnable()
    {
        InteractionTrigger.EnteredInteractionRange += InteractionPrompt_EnteredInteractionRange;
        InteractionTrigger.LeftInteractionRange += InteractionPrompt_LeftInteractionRange;
        InteractionTrigger.InteractionStarted += InteractionPrompt_InteractionStarted;
        InteractionTrigger.InteractionCompleted += InteractionPrompt_InteractionCompleted;
    }

    private void OnDisable()
    {
        InteractionTrigger.EnteredInteractionRange -= InteractionPrompt_EnteredInteractionRange;
        InteractionTrigger.LeftInteractionRange -= InteractionPrompt_LeftInteractionRange;
        InteractionTrigger.InteractionStarted -= InteractionPrompt_InteractionStarted;
        InteractionTrigger.InteractionCompleted -= InteractionPrompt_InteractionCompleted;
    }

    private void InteractionPrompt_EnteredInteractionRange()
    {
        ManagePrompt(true);
    }

    private void InteractionPrompt_LeftInteractionRange()
    {
        ManagePrompt(false);
    }

    private void InteractionPrompt_InteractionStarted()
    {
        ManagePrompt(false);
    }

    private void InteractionPrompt_InteractionCompleted()
    {
        ManagePrompt(true);
    }

    private void ManagePrompt(bool active)
    {
        _prompt.SetActive(active);
    }
}
