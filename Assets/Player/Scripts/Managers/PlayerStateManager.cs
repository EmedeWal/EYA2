using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    #region Enum
    public enum PlayerState
    {
        Idle,
        Moving,
        Dashing,
        Charging,
        Attacking,
        Recharging,
        Interacting,
        Paused
    }

    public PlayerState CurrentState;
    public PlayerState PreviousState;

    #endregion

    #region Setup

    private void OnEnable()
    {
        InteractionTrigger.LeftInteractionRange += PlayerStateManager_LeftInteractionRange;
        InteractionTrigger.InteractionStarted += PlayerStateManager_InteractionStarted;
        InteractionTrigger.InteractionCompleted += PlayerStateManager_InteractionCompleted;

        PlayerPause.Pause += PlayerStateManager_Pause;
        PlayerPause.Resume += PlayerStateManager_Resume;
    }

    private void OnDisable()
    {
        InteractionTrigger.LeftInteractionRange -= PlayerStateManager_LeftInteractionRange;
        InteractionTrigger.InteractionStarted -= PlayerStateManager_InteractionStarted;
        InteractionTrigger.InteractionCompleted -= PlayerStateManager_InteractionCompleted;

        PlayerPause.Pause -= PlayerStateManager_Pause;
        PlayerPause.Resume -= PlayerStateManager_Resume;
    }

    private void PlayerStateManager_LeftInteractionRange()
    {
        SetIdle();
    }

    private void PlayerStateManager_InteractionStarted()
    {
        SetInteracting();
    }

    private void PlayerStateManager_InteractionCompleted()
    {
        SetIdle();
    }

    private void PlayerStateManager_Pause()
    {
        SetPaused();
    }

    private void PlayerStateManager_Resume()
    {
        SetPreviousState();
    }

    #endregion

    #region Can Functions

    public bool CanMove()
    {
        return IsIdle() || IsMoving();
    }

    public bool CanRotate()
    {
        return IsIdle() || IsMoving() || IsCharging();
    }

    public bool CanDash()
    {
        return IsIdle() || IsMoving() || IsRecharging();
    }

    public bool CanAttack()
    {
        return IsIdle() || IsMoving() || IsRecharging();
    }

    public bool CanInteract()
    {
        return IsIdle() || IsMoving() || IsInteracting();
    }

    #endregion

    #region Is Functions

    public bool IsIdle()
    {
        return CurrentState == PlayerState.Idle;
    }

    public bool IsMoving()
    {
        return CurrentState == PlayerState.Moving;
    }

    public bool IsDashing()
    {
        return CurrentState == PlayerState.Dashing;
    }

    public bool IsCharging()
    {
        return CurrentState == PlayerState.Charging;
    }

    public bool IsAttacking()
    {
        return CurrentState == PlayerState.Attacking;
    }

    public bool IsRecharging()
    {
        return CurrentState == PlayerState.Recharging;
    }

    public bool IsInteracting()
    {
        return CurrentState == PlayerState.Interacting;
    }

    public bool IsPaused()
    {
        return CurrentState == PlayerState.Paused;
    }

    #endregion

    #region Set Functions

    public void SetPreviousState()
    {
        CurrentState = PreviousState;
    }

    public void SetState(PlayerState state)
    {
        PreviousState = CurrentState;
        CurrentState = state;
    }

    public void SetIdle()
    {
        SetState(PlayerState.Idle);
    }

    public void SetMoving()
    {
        SetState(PlayerState.Moving);
    }

    public void SetDashing()
    {
        SetState(PlayerState.Dashing);
    }

    public void SetCharging()
    {
        SetState(PlayerState.Charging);
    }

    public void SetAttacking()
    {
        SetState(PlayerState.Attacking);
    }

    public void SetRecharging()
    {
        SetState(PlayerState.Recharging);
    }

    public void SetInteracting()
    {
        SetState(PlayerState.Interacting);
    }

    public void SetPaused()
    {
        SetState(PlayerState.Paused);
    }

    #endregion
}
