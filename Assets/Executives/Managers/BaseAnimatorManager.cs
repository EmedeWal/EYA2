using System.Collections.Generic;
using UnityEngine;

namespace EmeWillem
{
    public abstract class BaseAnimatorManager : MonoBehaviour
    {
        [HideInInspector] public float MovementSpeed;
        [HideInInspector] public float AttackSpeed;

        [Header("ANIMATION PROPERTIES")]
        [SerializeField] private List<AnimationClipData> _animationClipDataList = new();
        private AnimationClipData _previousClipData = null;

        protected Animator _Animator;
        protected float _DeltaTime;

        private int _layerIndex = 1;
        private int _movementSpeedHash;
        private int _attackSpeedHash;
        private int _locomotionHash;
        private int _inActionHash;

        public virtual void Init(float movementSpeed = 1, float attackSpeed = 1)
        {
            _Animator = GetComponent<Animator>();

            MovementSpeed = movementSpeed;
            AttackSpeed = attackSpeed;

            foreach (var data in _animationClipDataList) data.Init();

            _movementSpeedHash = Animator.StringToHash("MovementSpeed");
            _attackSpeedHash = Animator.StringToHash("AttackSpeed");
            _locomotionHash = Animator.StringToHash("Locomotion");
            _inActionHash = Animator.StringToHash("InAction");

            _Animator.SetFloat(_movementSpeedHash, MovementSpeed);
            _Animator.SetFloat(_attackSpeedHash, AttackSpeed);
        }

        public virtual void Tick(float deltaTime, float locomotion, float transitionTime = 0.4f)
        {
            _DeltaTime = deltaTime;

            _Animator.SetFloat(_locomotionHash, locomotion, transitionTime, _DeltaTime);
            _Animator.SetFloat(_movementSpeedHash, MovementSpeed, 0.1f, _DeltaTime);
            _Animator.SetFloat(_attackSpeedHash, AttackSpeed, 0.1f, _DeltaTime);
        }

        public void ForceCrossFade(int animationHash)
        {
            AnimationClipData data = _animationClipDataList.Find(a => a.Hash == animationHash);
            if (data == null)
            {
                Debug.LogWarning("Animator Manager is not familiar with the provided hash.");
                return;
            }

            AnimatorStateInfo currentState = _Animator.GetCurrentAnimatorStateInfo(_layerIndex);
            if (currentState.shortNameHash != animationHash || data.AllowRepeat)
            {
                _Animator.CrossFadeInFixedTime(animationHash, data.TransitionDuration, _layerIndex, _DeltaTime);
            }
        }
   

        public void CrossFade(int animationHash)
        {
            if (_Animator.GetBool(_inActionHash)) return;

            AnimationClipData data = _animationClipDataList.Find(a => a.Hash == animationHash);
            if (data == null)
            {
                Debug.LogWarning("Animator Manager is not familiar with the provided hash.");
                return;
            }

            AnimatorStateInfo nextState = _Animator.GetNextAnimatorStateInfo(_layerIndex);
            AnimatorStateInfo currentState = _Animator.GetCurrentAnimatorStateInfo(_layerIndex);
            if ((currentState.shortNameHash != animationHash && nextState.shortNameHash != animationHash) || data.AllowRepeat)
            {
                if (_Animator.IsInTransition(_layerIndex) && !AllowOverride(currentState, nextState)) return;
                _Animator.CrossFadeInFixedTime(animationHash, data.TransitionDuration, _layerIndex, _DeltaTime);
                //_Animator.SetFloat(_locomotionHash, 0, nextState.length / 2, _DeltaTime);
                _previousClipData = data;
            }
        }

        public bool Idle()
        {
            return !(_Animator.GetBool(_inActionHash) || _Animator.IsInTransition(_layerIndex));
        }

        private bool AllowOverride(AnimatorStateInfo currentState, AnimatorStateInfo nextState)
        {
            if (currentState.shortNameHash == _previousClipData.Hash && _previousClipData.AllowOverrideDuringExit)
            {
                return true;
            }
            else if (nextState.shortNameHash == _previousClipData.Hash && _previousClipData.AllowOverrideDuringEnter)
            {
                return true;
            }
            return false;
        }

        #region External Access
        public void SetBool(int hash, bool value)
        {
            _Animator.SetBool(hash, value);
        }

        public float GetFloat(int hash)
        {
            return _Animator.GetFloat(hash);
        }

        public bool GetBool(int hash)
        {
            return _Animator.GetBool(hash);
        }
        #endregion
    }
}