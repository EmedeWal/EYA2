using UnityEngine;
public class IdleState : CreatureState
{
    public IdleState(CreatureAI creatureAI) : base(creatureAI) { }

    public override void Enter()
    {
        _CreatureAI.Locomotion.StopAgent(true);
    }

    public override void Tick(float delta)
    {
        Collider[] hitColliders = Physics.OverlapSphere(_CreatureAI.transform.position, _CreatureAI.CreatureData.WalkDistance, _CreatureAI.TargetLayer);

        if (hitColliders.Length > 0)
        {
            Transform nearestTarget = _CreatureAI.GetNearestTarget(hitColliders);
            if (nearestTarget != null)
            {
                _CreatureAI.SetState(new ChasingState(_CreatureAI, nearestTarget));
            }
        }
    }

    public override void Exit()
    {

    }
}