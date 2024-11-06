namespace EmeWillem
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "Combo Data", menuName = "Scriptable Object/Data/Combo Data/Guaranteed Crit")]
    public class GuaranteedCritCombo : ComboData
    {
        private PlayerAttackHandler _playerAttackHandler;

        public override void Init(VFXManager VFXManager, Transform transform)
        {
            base.Init(VFXManager, transform);

            _playerAttackHandler = _Transform.GetComponent<PlayerAttackHandler>();
        }

        protected override void PerformComboFinisher(AttackType attackType)
        {
            _playerAttackHandler.GuaranteedCrit = true;
        }
    }
}