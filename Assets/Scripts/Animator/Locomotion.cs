using UnityEngine;

namespace EmeWillem
{
    namespace Animation
    {
        public class Locomotion: StateMachineBehaviour
        {
            [Header("SETTINGS")]
            [SerializeField] private float _locomotionThreshold;
            [SerializeField] private float _newValue;
            private int _locomotionHash;

            private void Awake()
            {
                _locomotionHash = Animator.StringToHash("Locomotion");    
            }

            override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
            {
                if (animator.GetFloat(_locomotionHash) < _locomotionThreshold)
                {
                    animator.SetFloat(_locomotionHash, _newValue);
                }
            }
        }
    }
}