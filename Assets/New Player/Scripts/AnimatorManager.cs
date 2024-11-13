using UnityEngine;

namespace EmeWillem
{
    namespace Player
    {
        public class AnimatorManager : BaseAnimatorManager
        {
            [Header("SETTINGS")]
            [SerializeField] private float _snapThreshold = 0.05f;

            private Rigidbody _rigidbody;
            private int _horizontalHash;
            private int _verticalHash;
            private int _groundedHash;
            private int _lockedHash;

            private void OnAnimatorMove()
            {
                if (_DeltaTime != 0)
                {
                    Vector3 deltaPosition = _Animator.deltaPosition;
                    _rigidbody.velocity = deltaPosition / _DeltaTime;

                    Quaternion deltaRotation = _Animator.deltaRotation;
                    _rigidbody.MoveRotation(_rigidbody.rotation * deltaRotation);
                }
                else
                {
                    Debug.LogError("Delta Time was 0 while working with root motion");
                }
            }

            public override void Init(float movementSpeed = 1, float attackSpeed = 1)
            {
                base.Init(movementSpeed, attackSpeed);

                _rigidbody = GetComponentInParent<Rigidbody>();

                _horizontalHash = Animator.StringToHash("Horizontal");
                _verticalHash = Animator.StringToHash("Vertical");
                _groundedHash = Animator.StringToHash("Grounded");
                _lockedHash = Animator.StringToHash("Locked");
            }

            public void UpdateAnimatorValues(float deltaTime, float input, float horizontal, float vertical, float transitionTime, bool grounded, bool locked)
            {
                (input, horizontal, vertical) = (Snap(input), Snap(horizontal), Snap(vertical));

                base.Tick(deltaTime, input, transitionTime);

                _Animator.SetFloat(_horizontalHash, horizontal, transitionTime, deltaTime);
                _Animator.SetFloat(_verticalHash, vertical, transitionTime, deltaTime);
                _Animator.SetBool(_groundedHash, grounded);
                _Animator.SetBool(_lockedHash, locked);
            }

            private float Snap(float value)
            {
                value = Mathf.Abs(value - 0.9f) < _snapThreshold ? 0.9f :
                             Mathf.Abs(value - 0.5f) < _snapThreshold ? 0.5f :
                             Mathf.Abs(value - 0.1f) < _snapThreshold ? 0.1f : value;

                return value;
            }
        }
    }
}