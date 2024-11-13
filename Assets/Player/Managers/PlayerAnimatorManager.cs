//using UnityEngine;

//public class PlayerAnimatorManager : BaseAnimatorManager
//{
//    private int _animatorHorizontal;
//    private int _animatorVertical;

//    public override void Init(float movementSpeed = 1, float attackSpeed = 1)
//    {
//        base.Init(movementSpeed, attackSpeed);

//        _animatorHorizontal = _Animator.StringToHash("Horizontal");
//        _animatorVertical = _Animator.StringToHash("Vertical");
//    }

//    public void UpdateAnimatorValues(float deltaTime, float horizontal, float vertical, bool grounded, bool locked, bool moving)
//    {
//        if (!grounded)
//        {
//            horizontal = 0;
//            vertical = 0;
//        }

//        float locomotion = moving ? 1 : 0;

//        base.Tick(deltaTime, locomotion);

//        _Animator.SetFloat(_animatorHorizontal, horizontal, 0.1f, deltaTime);
//        _Animator.SetFloat(_animatorVertical, vertical, 0.1f, deltaTime);
//        _Animator.SetBool("Grounded", grounded);
//        _Animator.SetBool("Locked", locked);
//    }
//}