using UnityEngine;

namespace EmeWillem
{
    namespace Animation
    {
        public class Action : StateMachineBehaviour
        {
            private int _inActionHash;

            private void Awake()
            {
                _inActionHash = Animator.StringToHash("InAction");
            }

            public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
            {
                animator.SetBool(_inActionHash, true);
            }

            public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
            {
                animator.SetBool(_inActionHash, false);
            }
        }
    }
}