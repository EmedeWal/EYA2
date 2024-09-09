using UnityEngine;

namespace EW
{
    public class Helper : MonoBehaviour
    {
        private Animator _animator;

        public string AnimationName;
        public bool PlayAnimation;
        public bool LockedOn;

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            if (PlayAnimation)
            {
                _animator.CrossFade(AnimationName, 0.1f, 1);
                PlayAnimation = false;
            }
        }
    }
}
