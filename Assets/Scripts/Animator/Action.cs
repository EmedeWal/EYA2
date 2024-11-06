using UnityEngine;

namespace EmeWillem
{
    namespace Animation
    {
        public class Action : StateMachineBehaviour
        {
            override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
            {
                animator.SetBool("CanRotate", false);
            }

            override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
            {
                animator.SetBool("CanRotate", true);
            }
        }
    }
}