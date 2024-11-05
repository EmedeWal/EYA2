using UnityEngine;

public class Attack : StateMachineBehaviour
{
    public delegate void StateEntered_Delegate(int animationHash);
    public static event StateEntered_Delegate StateEntered;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnStateEntered(stateInfo.tagHash);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("CanRotate", true);
    }

    private void OnStateEntered(int animationHash)
    {
        StateEntered?.Invoke(animationHash);
    }
}