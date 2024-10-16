using UnityEngine;
public class IdleState : CreatureState
{
    public IdleState(CreatureAI creatureAI) : base(creatureAI) { }

    public override void Tick(float delta)
    {
        Transform nearestTarget = _CreatureAI.GetNearestTarget(_CreatureAI.CreatureData.RunDistance);
        if (nearestTarget != null)
        {
            _CreatureAI.SetState(new ChasingState(_CreatureAI, nearestTarget));
        }
        else if (_CreatureAI.DefaultTarget != null)
        {
            _CreatureAI.SetState(new ChasingState(_CreatureAI, _CreatureAI.DefaultTarget));
        }
    }
}