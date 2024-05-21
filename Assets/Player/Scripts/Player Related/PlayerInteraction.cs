using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private PlayerStateManager _stateManager;
    private PlayerInputManager _inputManager;

    public delegate void PlayerInteraction_Interaction();
    public static event PlayerInteraction_Interaction Interaction;

    private float _interactionCooldown = 0.2f;
    private bool _canInteract = true;

    private void Awake()
    {
        _stateManager = GetComponent<PlayerStateManager>();
        _inputManager = GetComponent<PlayerInputManager>();
    }

    private void OnEnable()
    {
        _inputManager.InteractionInput_Performed += PlayerInteraction_InteractionInput_Performed;
    }

    private void OnDisable()
    {
        _inputManager.InteractionInput_Performed -= PlayerInteraction_InteractionInput_Performed;
    }

    private void PlayerInteraction_InteractionInput_Performed()
    {
        if (_stateManager.CanInteract() && _canInteract)
        {
            _canInteract = false;
            Invoke(nameof(ResetCanInteract), _interactionCooldown);
            OnInteraction();
        }
    }

    private void OnInteraction()
    {
        Interaction?.Invoke();
    }

    private void ResetCanInteract()
    {
        _canInteract = true;
    }
}
