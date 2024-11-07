using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace EmeWillem
{
    namespace Player
    {
        public class AttackHandler : BaseAttackHandler
        {
            private InputHandler _inputHandler;
            private Locomotion _locomotion;

            [Header("ANIMATION STRINGS")]
            [SerializeField] private string _lightW = "Right Slash 2 [LW0]";
            [SerializeField] private string _heavyW = "Right Slash 1 [HW0]";
            [SerializeField] private string _lightD = "Sweep 1 [LD0]";
            [SerializeField] private string _heavyD = "Leap 1 [HD0]";

            [Header("SETTINGS")]
            [SerializeField] private float _comboTime = 0.2f;

            public override void Init(List<OffenseCollider> offenseColliders)
            {
                base.Init(offenseColliders);

                _inputHandler = GetComponent<InputHandler>();
                _locomotion = GetComponent<Locomotion>();

                _inputHandler.LightAttackInputPerformed += AttackHandler_LightAttackInputPerformed;
                _inputHandler.HeavyAttackInputPerformed += AttackHandler_HeavyAttackInputPerformed;
            }

            public void Cleanup()
            {
                _inputHandler.LightAttackInputPerformed -= AttackHandler_LightAttackInputPerformed;
                _inputHandler.HeavyAttackInputPerformed -= AttackHandler_HeavyAttackInputPerformed;
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
                    _AnimatorManager.CrossFade(dashing);
                }
                else
                {
                    _AnimatorManager.CrossFade(walking);
                }
            }

            private IEnumerator ManageAnimatorCoroutine(float delay, string buttonPressed, string buttonNotPressed)
            {
                _AnimatorManager.SetBool(buttonNotPressed, false);
                _AnimatorManager.SetBool(buttonPressed, true);

                yield return new WaitForSeconds(delay);

                _AnimatorManager.SetBool(buttonPressed, false);
            }
        }
    }
}