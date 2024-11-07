namespace EmeWillem
{
    namespace AI
    {
        public abstract class EnemyState
        {
            protected Enemy _Enemy;

            public EnemyState(Enemy enemy)
            {
                _Enemy = enemy;
            }

            public virtual void Enter() { }
            public virtual void Tick(float delta) { }
            public virtual void Exit() { }
        }
    }
}