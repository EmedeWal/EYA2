using UnityEngine;

namespace EmeWillem
{
    namespace Animation
    {
        //public class Locomotion: StateMachineBehaviour
        //{
        //    [Header("SETTINGS")]
        //    [SerializeField] private float _locomotionThreshold = 1;
        //    [SerializeField] private float _newValue = 0;
        //    [SerializeField] private bool _limiter = false;
        //    private int _locomotionHash;
        //    private float _startingLocomotionValue;

        //    private void Awake()
        //    {
        //        _locomotionHash = Animator.StringToHash("Locomotion");    
        //    }

        //    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //    {
        //        _startingLocomotionValue = animator.GetFloat(_locomotionHash);
        //        //float locomotion = animator.GetFloat(_locomotionHash);
        //        //if (_limiter && locomotion > _locomotionThreshold)
        //        //{
        //        //    animator.SetFloat(_locomotionHash, _newValue);
        //        //}
        //        //else if (locomotion < _locomotionThreshold)
        //        //{
        //        //    animator.SetFloat(_locomotionHash, _newValue);
        //        //}
        //    }

        //    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //    {
        //        animator.SetFloat(_locomotionHash, _startingLocomotionValue);
        //    }
        //}
    }
}