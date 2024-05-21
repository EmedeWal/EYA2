using UnityEngine;

public class StanceShop : InteractionTrigger
{
    #region Activity Management

    [Header("ACTIVITY MANAGEMENT")]
    [SerializeField] private bool _activeOnStart;
    [SerializeField] private GameObject _lightObject;
    [SerializeField] private Collider _trigger;

    public void Enable()
    {
        _lightObject.SetActive(true);
        _trigger.enabled = true;    
    }

    public void Disable()
    {
        Destroy(_lightObject);
        Destroy(_trigger);
    }

    #endregion

    public delegate void StanceShop_Interaction();
    public static event StanceShop_Interaction Interaction;

    private bool _interactionActive = false;

    private void Start()
    {
        _lightObject.SetActive(_activeOnStart);
        _trigger.enabled = _activeOnStart;
    }

    private void OnDestroy()
    {
        Disable();
    }

    private void OnEnable()
    {
        InteractionDetected += StanceShop_InteractionDetected;

        StancePurchaseMenu.UnlockStance += StanceShop_UnlockStance;
    }

    private void OnDisable()
    {
        InteractionDetected -= StanceShop_InteractionDetected;

        StancePurchaseMenu.UnlockStance -= StanceShop_UnlockStance;
    }

    private void StanceShop_InteractionDetected()
    {
        if (_interactionActive)
        {
            _interactionActive = false;
            OnInteractionCompleted();
        }
        else
        {
            _interactionActive = true;
            OnInteractionStarted();
        }

        OnInteraction();
    }

    private void StanceShop_UnlockStance(StanceType stanceType)
    {
        if (InRange)
        {
            OnInteractionCompleted();
            OnLeftInteractionRange();
            Destroy(this);
        }
    }

    private void OnInteraction()
    {
        Interaction?.Invoke();
    }
}
