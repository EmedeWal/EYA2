using System;
using UnityEngine;

public abstract class InteractionTrigger : MonoBehaviour
{
    public delegate void InteractionTrigger_EnteredInteractionRange();
    public static event InteractionTrigger_EnteredInteractionRange EnteredInteractionRange;

    public delegate void InteractionTrigger_LeftInteractionRange();
    public static event InteractionTrigger_LeftInteractionRange LeftInteractionRange;

    public delegate void InteractionTrigger_InteractionStarted();
    public static event InteractionTrigger_InteractionStarted InteractionStarted;

    public delegate void InteractionTrigger_InteractionCompleted();
    public static event InteractionTrigger_InteractionCompleted InteractionCompleted;

    protected event Action InteractionDetected;

    protected bool InRange = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            InRange = true;
            PlayerInteraction.Interaction += InteractionTrigger_Interaction;
            OnEnteredInteractionRange();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            InRange = false;
            PlayerInteraction.Interaction -= InteractionTrigger_Interaction;
            OnLeftInteractionRange();
        }
    }

    private void InteractionTrigger_Interaction()
    {
        OnInteractionDetected();
    }

    private void OnEnteredInteractionRange()
    {
        EnteredInteractionRange?.Invoke();
    }

    protected void OnLeftInteractionRange()
    {
        LeftInteractionRange?.Invoke();
    }

    protected void OnInteractionStarted()
    {
        InteractionStarted?.Invoke();
    }

    protected void OnInteractionCompleted()
    {
        InteractionCompleted?.Invoke();
    }

    private void OnInteractionDetected()
    {
        InteractionDetected?.Invoke();
    }
}
