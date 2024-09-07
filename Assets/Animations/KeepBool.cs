using UnityEngine;

public class KeepBool : StateMachineBehaviour
{
    public string BoolName;
    public bool Status;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(BoolName, Status);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(BoolName, !Status);
    }
}
