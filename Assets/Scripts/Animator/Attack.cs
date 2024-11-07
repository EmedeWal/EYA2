using UnityEngine;

namespace EmeWillem
{
    namespace Animation
    {
        public class Attack : StateMachineBehaviour
        {
            [Header("DATA REFERENCE")]
            [SerializeField] private BaseAttackData _attackData;
            private BaseAttackHandler _attackHandler;

            public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
            {
                _attackHandler = animator.GetComponent<BaseAttackHandler>();
                _attackHandler.EnterAttackingState(_attackData);
            }

            public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
            {
                animator.SetBool("CanRotate", true);
                _attackHandler.LeaveAttackingState(_attackData);
            }
        }
    }
}