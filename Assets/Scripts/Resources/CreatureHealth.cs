namespace EmeWillem
{
    using UnityEngine;


    public class CreatureHealth : Health
    {
        //[Header("STAGGER")]
        //[SerializeField] private float _staggerRecovery;
        //[SerializeField] private float _maximumPosture;

        //private CreatureAnimatorManager _AnimatorManager;
        //private float _currentStaggerValue;

        //public override void Init(int maxValue, int currentValue)
        //{
        //    base.Init(maxValue, currentValue);

        //    //_AnimatorManager = GetComponent<CreatureAnimatorManager>();

        //    //_currentStaggerValue = 0;
        //}

        public override void LateTick()
        {
            base.LateTick();

        }
    }
}