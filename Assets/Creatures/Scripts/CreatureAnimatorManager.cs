using System;

public class CreatureAnimatorManager : AnimatorManager
{
    public event Action SpawnAnimationFinished;

    public void OnSpawnAnimationFinished()
    {
        SpawnAnimationFinished?.Invoke();
    }
}