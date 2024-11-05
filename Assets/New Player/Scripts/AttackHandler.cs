using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace Player
{
    public class AttackHandler : MonoBehaviour
    {
        private InputHandler _inputHandler;
        private AnimatorManager _animatorManager;
        private LayerMask _targetLayer;

        [Header("ATTACK ORIGINS")]
        [SerializeField] private List<DamageCollider> _damageColliderList = new();

        [Header("ATTACK LIST")]
        [SerializeField] private List<AttackData> _attackDataList;
        private AttackData _currentAttackData;

        [Header("SETTINGS")]
        [SerializeField] private float _comboTime = 0.2f;

        public void Init()
        {
            _inputHandler = GetComponent<InputHandler>();
            _animatorManager = GetComponent<AnimatorManager>();
            _targetLayer = LayerMask.GetMask("DamageCollider");

            foreach (DamageCollider collider in _damageColliderList)
            {
                collider.Init(_targetLayer);
            }

            _inputHandler.LightAttackInputPerformed += AttackHandler_LightAttackInputPerformed;
            _inputHandler.HeavyAttackInputPerformed += AttackHandler_HeavyAttackInputPerformed;
        }

        public void Cleanup()
        {
            _inputHandler.LightAttackInputPerformed -= AttackHandler_LightAttackInputPerformed;
            _inputHandler.HeavyAttackInputPerformed -= AttackHandler_HeavyAttackInputPerformed;
        }

        public void AttackStart(string name)
        {
            _animatorManager.SetBool("CanRotate", false);

            foreach (DamageCollider colider in _damageColliderList)
            {
                if (colider.Name == name)
                {
                    colider.Activate(_currentAttackData.Damage);
                    break;
                }
            }
        }

        public void AttackEnd(string name)
        {
            foreach (DamageCollider colider in _damageColliderList)
            {
                if (colider.Name == name)
                {
                    colider.Deactivate();
                    break;
                }
            }
        }

        private void AttackHandler_LightAttackInputPerformed()
        {
            StopAllCoroutines();
            Attack(_attackDataList[0]);
            StartCoroutine(ManageAnimatorCoroutine(_comboTime, "R1Press"));

            _animatorManager.SetBool("R2Press", false);
        }

        private void AttackHandler_HeavyAttackInputPerformed()
        {
            StopAllCoroutines();
            Attack(_attackDataList[1]);
            StartCoroutine(ManageAnimatorCoroutine(_comboTime, "R2Press"));

            _animatorManager.SetBool("R1Press", false);
        }

        private void Attack(AttackData attackData)
        {
            _currentAttackData = attackData;
            _animatorManager.CrossFade(Time.deltaTime, _currentAttackData.AnimationName);
        }

        private IEnumerator ManageAnimatorCoroutine(float delay, string button)
        {
            _animatorManager.SetBool(button, true);

            yield return new WaitForSeconds(delay);

            _animatorManager.SetBool(button, false);
        }
    }
}