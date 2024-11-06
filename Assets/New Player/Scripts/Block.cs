using EmeWillem.Utilities;
using UnityEngine;

namespace EmeWillem
{
    namespace Player
    {
        public class Block : MonoBehaviour
        {
            private InputHandler _inputHandler;
            private AnimatorManager _animatorManager;
            private AttackHandler _attackHandler;

            public void Init()
            {
                _inputHandler = GetComponent<InputHandler>();
                _animatorManager = GetComponent<AnimatorManager>();
                _attackHandler = GetComponent<AttackHandler>();

                _inputHandler.BlockInputPerformed += Block_BlockInputPerformed;
                _inputHandler.BlockInputCanceled += Block_BlockInputCanceled;

                AttackEventHelper.OnBlockCheck += IsBlocking;

                _animatorManager.SetBool("Blocking", true);
            }

            public void Cleanup()
            {
                _inputHandler.BlockInputPerformed -= Block_BlockInputPerformed;
                _inputHandler.BlockInputCanceled -= Block_BlockInputCanceled;

                AttackEventHelper.OnBlockCheck -= IsBlocking;
            }

            private void Block_BlockInputPerformed()
            {
                if (!_attackHandler.Attacking)
                {
                    _animatorManager.ForceCrossFade("Block");
                }
            }

            private void Block_BlockInputCanceled()
            {
                _animatorManager.SetBool("Blocking", false);
            }

            private bool IsBlocking(GameObject targetObject, Vector3 attackerPosition)
            {
                if (targetObject != gameObject) return false;

                Vector3 toAttacker = attackerPosition - transform.position;
                toAttacker.y = 0; 

                float angleToAttacker = Vector3.Angle(transform.forward, toAttacker);
                bool isBlocking = _animatorManager.GetBool("Blocking");
                bool attackerInFront = angleToAttacker <= 45f;

                if (isBlocking && attackerInFront)
                {
                    _animatorManager.ForceCrossFade("Deflect");
                    return true;
                }
                return false;
            }
        }
    }
}