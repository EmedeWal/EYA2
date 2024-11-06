using UnityEngine;

namespace EmeWillem
{
    namespace Animation
    { 
        public class Block : StateMachineBehaviour
        {
            public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
            {
                animator.SetBool("Blocking", true);
            }
        }
    }
}