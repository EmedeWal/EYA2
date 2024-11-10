using UnityEngine;

namespace EmeWillem
{
    namespace Animation
    {
        public class Empty : StateMachineBehaviour
        {
            private int _inActionHash;

            private void Awake()
            {
                _inActionHash = Animator.StringToHash("InAction");
            }

            override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
            {
                animator.SetBool(_inActionHash, false);
            }
        }
    }
}