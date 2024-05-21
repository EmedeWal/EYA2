using UnityEngine;

public class SkeletonAI : MeleeEnemyAI
{
    [Header("ATTACK DATA")]
    [SerializeField] private EnemyMeleeData _attackData;

    private void Awake()
    {
        DetermineAttackData();
        SetReferences();
    }
}

