using UnityEngine;

namespace EmeWillem
{
    namespace Animation
    {
        public class Empty : StateMachineBehaviour
        {
            private int _keepLocomotionHash;
            private int _locomotionHash;
            private int _horizontalHash;
            private int _verticalHash;
            private int _inActionHash;

            private void Awake()
            {
                _keepLocomotionHash = Animator.StringToHash("KeepLocomotion");
                _locomotionHash = Animator.StringToHash("Locomotion");
                _horizontalHash = Animator.StringToHash("Horizontal");
                _verticalHash = Animator.StringToHash("Vertical");
                _inActionHash = Animator.StringToHash("InAction");
            }

            override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
            {
                animator.SetBool(_inActionHash, false);
            }

            override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
            {
                animator.SetBool(_inActionHash, true);
                //AnimatorStateInfo nextState = stateInfo;
                //AnimatorStateInfo nextState = animator.GetNextAnimatorStateInfo(layerIndex);
                //float dampTime = nextState.length / 2;
                //float deltaTime = Time.fixedDeltaTime;

                //Debug.Log(nextState.shortNameHash);
                //if (nextState.tagHash != _horizontalHash)
                //{
                //    animator.SetFloat(_locomotionHash, 0, dampTime, deltaTime);
                //    animator.SetFloat(_horizontalHash, 0, dampTime, deltaTime);
                //    animator.SetFloat(_verticalHash, 0, dampTime, deltaTime);
                //}
            }
        }
    }
}