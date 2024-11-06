using UnityEngine;

namespace EmeWillem
{
    namespace Animation
    {
        public class Empty : StateMachineBehaviour
        {
            override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
            {
                animator.SetBool("InAction", false);
            }

            override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
            {
                animator.SetBool("InAction", true);
            }
        }
    }
}