using System;

public class CreatureAnimatorManager : BaseAnimatorManager
{
    public event Action SpawnAnimationFinished;

    public void OnSpawnAnimationFinished()
    {
        SpawnAnimationFinished?.Invoke();
    }
}