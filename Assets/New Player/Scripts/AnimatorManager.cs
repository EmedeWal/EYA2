using UnityEngine;

namespace Player
{
    public class AnimatorManager : BaseAnimatorManager
    {
        private int _animatorHorizontal;
        private int _animatorVertical;

        public override void Init(float movementSpeed = 1, float attackSpeed = 1)
        {
            base.Init(movementSpeed, attackSpeed);

            _animatorHorizontal = Animator.StringToHash("Horizontal");
            _animatorVertical = Animator.StringToHash("Vertical");

            Animator.SetBool("CanRotate", true);
        }

        public void Cleanup()
        {

        }

        public void UpdateAnimatorValues(float delta, float locomotion, float horizontal, float vertical, bool grounded, bool locked)
        {
            if (!grounded)
            {
                locomotion = 0;
                horizontal = 0;
                vertical = 0;
            }

            locomotion = SnapMovementValue(locomotion);
            horizontal = SnapMovementValue(horizontal);
            vertical = SnapMovementValue(vertical);

            if (locked)
            {
                float absHorizontal = Mathf.Abs(horizontal);
                float absVertical = Mathf.Abs(vertical);

                if (absHorizontal > absVertical)
                {
                    vertical = 0;
                    horizontal = horizontal > 0 ? 1 : -1;
                }
                else if (absVertical > absHorizontal)
                {
                    horizontal = 0;
                    vertical = vertical > 0 ? 1 : -1;
                }
                else
                {
                    if (absHorizontal != 0 && absVertical != 0 && absHorizontal - absVertical < 0.1)
                    {
                        if (vertical < 0 && horizontal > 0)
                        {
                            horizontal = 0;
                            vertical = -1;
                        }

                        if (horizontal < 0 && vertical > 0)
                        {
                            vertical = 0;
                            horizontal = -1;
                        }
                    }
                }

            }


            base.Tick(delta, locomotion);

            Animator.SetFloat(_animatorHorizontal, horizontal, 0.1f, _Delta);
            Animator.SetFloat(_animatorVertical, vertical, 0.1f, _Delta);
            Animator.SetBool("Grounded", grounded);
            Animator.SetBool("Locked", locked);
        }

        private float SnapMovementValue(float movementValue)
        {
            float snapThreshold = 0.05f;

            if (Mathf.Abs(movementValue - 1f) < snapThreshold)
            {
                return 1f;
            }
            else if (Mathf.Abs(movementValue - 0.5f) < snapThreshold)
            {
                return 0.5f;
            }
            else if (Mathf.Abs(movementValue) < snapThreshold)
            {
                return 0f;
            }

            return movementValue;
        }
    }
}