namespace EmeWillem
{
    public abstract class CreatureState
    {
        protected CreatureAI _CreatureAI;

        public CreatureState(CreatureAI creatureAI)
        {
            _CreatureAI = creatureAI;
        }

        public virtual void Enter() { }
        public virtual void Tick(float delta) { }
        public virtual void Exit() { }
    }
}