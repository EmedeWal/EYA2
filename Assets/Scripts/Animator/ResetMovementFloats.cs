using UnityEngine;

namespace EmeWillem
{
    namespace Animation
    {
        public class ResetMovementFloats : StateMachineBehaviour
        {
            private int _locomotionHash;
            private int _horizontalHash;
            private int _verticalHash;

            private void Awake()
            {
                _locomotionHash = Animator.StringToHash("Locomotion");
                _horizontalHash = Animator.StringToHash("Horizontal");
                _verticalHash = Animator.StringToHash("Vertical");
            }

            override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
            {
                animator.SetFloat(_locomotionHash, 0);
                animator.SetFloat(_horizontalHash, 0);
                animator.SetFloat(_verticalHash, 0);
            }
        }
    }
}