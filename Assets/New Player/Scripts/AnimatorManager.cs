using UnityEngine;

namespace EmeWillem
{
    namespace Player
    {
        public class AnimatorManager : BaseAnimatorManager
        {
            private Rigidbody _rigidbody;
            private int _groundedHash;

            private void OnAnimatorMove()
            {
                Vector3 deltaPosition = Animator.deltaPosition;
                _rigidbody.velocity = deltaPosition / _DeltaTime;

                Quaternion deltaRotation = Animator.deltaRotation;
                _rigidbody.MoveRotation(_rigidbody.rotation * deltaRotation);
            }

            public override void Init(float movementSpeed = 1, float attackSpeed = 1)
            {
                base.Init(movementSpeed, attackSpeed);

                _rigidbody = GetComponentInParent<Rigidbody>();

                _groundedHash = Animator.StringToHash("Grounded");
            }

            public void UpdateAnimatorValues(float deltaTime, float input, bool grounded)
            {
                input = SnapMovementValue(input, grounded);

                base.Tick(deltaTime, input);

                Animator.SetBool(_groundedHash, grounded);
            }

            private float SnapMovementValue(float locomotion, bool grounded)
            {
                if (!grounded)
                {
                    return 0f;
                }

                float snapThreshold = 0.05f;

                locomotion = Mathf.Abs(locomotion - 1f) < snapThreshold ? 1f :
                             Mathf.Abs(locomotion - 0.5f) < snapThreshold ? 0.5f :
                             Mathf.Abs(locomotion) < snapThreshold ? 0f : locomotion;

                return locomotion;
            }
        }
    }
}