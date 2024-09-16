using UnityEngine;

public class PlayerAnimatorManager : MonoBehaviour
{
    public Animator _Animator {  get; private set; }

    private int _animatorHorizontal;
    private int _animatorVertical;

    public void Init()
    {
        _Animator = GetComponentInChildren<Animator>();

        _animatorHorizontal = Animator.StringToHash("Horizontal");
        _animatorVertical = Animator.StringToHash("Vertical");
    }

    public void UpdateAnimatorValues(float delta, float horizontal, float vertical, bool grounded, bool lockedOn)
    {
        if (!grounded)
        {
            horizontal = 0;
            vertical = 0;
        }

        _Animator.SetFloat(_animatorHorizontal, horizontal, 0.1f, delta);
        _Animator.SetFloat(_animatorVertical, vertical, 0.1f, delta);
        _Animator.SetBool("LockedOn", lockedOn);
    }

    public void PlayAnimation(string animationName)
    {
        _Animator.Play(animationName);
    }

    public void CrossFadeAnimation(float delta, string animationName, float transitionDuration, int layer)
    {
        _Animator.CrossFade(animationName, transitionDuration, layer, delta);
    }

    public bool GetBool(string boolName)
    {
        return _Animator.GetBool(boolName);
    }
}
