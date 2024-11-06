using UnityEngine;

namespace EmeWillem
{
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

            public void UpdateAnimatorValues(float delta, float locomotion, float horizontal, float vertical, bool locked, bool grounded)
            {
                (locomotion, horizontal, vertical) = SnapMovementValues(locomotion, horizontal, vertical, locked, grounded);

                base.Tick(delta, locomotion);

                Animator.SetFloat(_animatorHorizontal, horizontal, 0.1f, _DeltaTime);
                Animator.SetFloat(_animatorVertical, vertical, 0.1f, _DeltaTime);
                Animator.SetBool("Grounded", grounded);
                Animator.SetBool("Locked", locked);
            }

            private (float locomotion, float horizontal, float vertical) SnapMovementValues(float locomotion, float horizontal, float vertical, bool locked, bool grounded)
            {
                if (!grounded)
                {
                    return (0f, 0f, 0f);
                }

                float snapThreshold = 0.05f;

                locomotion = Mathf.Abs(locomotion - 1f) < snapThreshold ? 1f :
                             Mathf.Abs(locomotion - 0.5f) < snapThreshold ? 0.5f :
                             Mathf.Abs(locomotion) < snapThreshold ? 0f : locomotion;

                horizontal = Mathf.Abs(horizontal - 1f) < snapThreshold ? 1f :
                             Mathf.Abs(horizontal + 1f) < snapThreshold ? -1f :
                             Mathf.Abs(horizontal) < snapThreshold ? 0f : horizontal;

                vertical = Mathf.Abs(vertical - 1f) < snapThreshold ? 1f :
                           Mathf.Abs(vertical + 1f) < snapThreshold ? -1f :
                           Mathf.Abs(vertical) < snapThreshold ? 0f : vertical;

                if (locked)
                {
                    float absHorizontal = Mathf.Abs(horizontal);
                    float absVertical = Mathf.Abs(vertical);

                    if (absHorizontal > absVertical)
                    {
                        vertical = 0;
                        horizontal = horizontal > 0 ? 1 : -1;
                    }
                    else if (absHorizontal < absVertical)
                    {
                        horizontal = 0;
                        vertical = vertical > 0 ? 1 : -1;
                    }
                    else
                    {
                        if (vertical < 0 && horizontal > 0)
                        {
                            horizontal = 0;
                            vertical = -1;
                        }
                        else if (horizontal < 0 && vertical > 0)
                        {
                            vertical = 0;
                            horizontal = -1;
                        }
                    }
                }

                return (locomotion, horizontal, vertical);
            }
        }
    }
}