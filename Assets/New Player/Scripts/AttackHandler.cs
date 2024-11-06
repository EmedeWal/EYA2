using System.Collections.Generic;
using EmeWillem.Animation;
using System.Collections;
using UnityEngine;

namespace EmeWillem
{
    namespace Player
    {
        public class AttackHandler : MonoBehaviour
        {
            public bool Attacking { get; private set; }

            private InputHandler _inputHandler;
            private AnimatorManager _animatorManager;
            private Locomotion _locomotion;
            private LayerMask _targetLayer;

            [Header("ATTACK ORIGINS")]
            [SerializeField] private List<OffenseCollider> _hitColliderList = new();

            [Header("ATTACK LIST")]
            [SerializeField] private List<AttackData> _attackDataList;
            private AttackData _attackData;

            [Header("ANIMATION STRINGS")]
            [SerializeField] private string _lightW = "Right Slash 2 [LW0]";
            [SerializeField] private string _heavyW = "Right Slash 1 [HW0]";
            [SerializeField] private string _lightD = "Sweep 1 [LD0]";
            [SerializeField] private string _heavyD = "Leap 1 [HD0]";

            [Header("SETTINGS")]
            [SerializeField] private float _comboTime = 0.2f;

            public void Init()
            {
                _inputHandler = GetComponent<InputHandler>();
                _animatorManager = GetComponent<AnimatorManager>();
                _locomotion = GetComponent<Locomotion>();
                _targetLayer = LayerMask.GetMask("DamageCollider");

                foreach (OffenseCollider offenseCollider in _hitColliderList)
                {
                    offenseCollider.Init(transform, _targetLayer);

                    foreach (AttackData attackData in _attackDataList)
                    {
                        if (offenseCollider.OffenseColliderType == attackData.OffenseColliderType)
                        {
                            attackData.OffenseCollider = offenseCollider;
                        }
                    }
                }

                _inputHandler.LightAttackInputPerformed += AttackHandler_LightAttackInputPerformed;
                _inputHandler.HeavyAttackInputPerformed += AttackHandler_HeavyAttackInputPerformed;

                Attack.StateEntered += AttackHandler_StateEntered;
            }

            public void Cleanup()
            {
                _inputHandler.LightAttackInputPerformed -= AttackHandler_LightAttackInputPerformed;
                _inputHandler.HeavyAttackInputPerformed -= AttackHandler_HeavyAttackInputPerformed;

                Attack.StateEntered -= AttackHandler_StateEntered;
            }

            private void AttackHandler_StateEntered(int animationHash)
            {
                _attackData = _attackDataList.Find(data => data.AnimationHash == animationHash);

                // Something regarding audio, but no audio system here yet
            }

            public void AttackStart()
            {
                _attackData.OffenseCollider.Activate(_attackData.Damage, _attackData.Stagger);
                _animatorManager.SetBool("CanRotate", false);
                Attacking = true;
            }

            public void AttackEnd()
            {
                _attackData.OffenseCollider.Deactivate();
                Attacking = false;
            }

            private void AttackHandler_LightAttackInputPerformed()
            {
                UseAttack(_lightW, _lightD, "R1Press", "R2Press");
            }

            private void AttackHandler_HeavyAttackInputPerformed()
            {
                UseAttack(_heavyW, _heavyD, "R2Press", "R1Press");
            }

            private void UseAttack(string walking, string dashing, string buttonPressed, string buttonNotPressed)
            {
                StopAllCoroutines();
                StartCoroutine(ManageAnimatorCoroutine(_comboTime, buttonPressed, buttonNotPressed));

                if (_locomotion.Dashing)
                {
                    _animatorManager.CrossFade(dashing);
                }
                else
                {
                    _animatorManager.CrossFade(walking);
                }
            }

            private IEnumerator ManageAnimatorCoroutine(float delay, string buttonPressed, string buttonNotPressed)
            {
                _animatorManager.SetBool(buttonNotPressed, false);
                _animatorManager.SetBool(buttonPressed, true);

                yield return new WaitForSeconds(delay);

                _animatorManager.SetBool(buttonPressed, false);
            }
        }
    }
}