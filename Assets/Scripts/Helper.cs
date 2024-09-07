using UnityEngine;

namespace EW
{
    public class Helper : MonoBehaviour
    {
        [Range(-1, 1)] public float Horizontal;
        [Range(-1, 1)] public float Vertical;    

        private Animator _animator;

        public string AnimationName;
        public bool PlayAnimation;
        public bool LockedOn;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            //Horizontal = Animator.StringToHash("Horizontal");
            //Vertical = Animator.StringToHash("Vertical");
        }

        //private void Start()
        //{
        //    _animator.CrossFade("LightAttack", 0.2f);
        //}

        private void Update()
        {
            if (PlayAnimation)
            {
                Vertical = 0;
                _animator.CrossFade(AnimationName, 0.1f, 1);
                PlayAnimation = false;
            }

            _animator.SetBool("LockedOn", LockedOn);
            _animator.SetFloat("Horizontal", Horizontal);
            _animator.SetFloat("Vertical", Vertical);
        }
    }
}
