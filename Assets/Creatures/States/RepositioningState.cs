using UnityEngine;

public class RepositioningState : CreatureState
{
    private Transform _target;
    private Vector3 _repositionTarget;

    public RepositioningState(CreatureAI creatureAI, Transform target) : base(creatureAI)
    {
        _target = target;
        _repositionTarget = GetRepositionTarget();
    }

    public override void Enter()
    {
        _CreatureAI.Locomotion.StopAgent(false);
        _CreatureAI.Locomotion.SetDestination(_repositionTarget);
    }

    public override void Tick(float delta)
    {
        if (Vector3.Distance(_CreatureAI.transform.position, _repositionTarget) < 1f)
        {
            _CreatureAI.SetState(new ChasingState(_CreatureAI, _target));
        }
    }

    private Vector3 GetRepositionTarget()
    {
        Vector3 direction = (_CreatureAI.transform.position - _target.position).normalized;
        return _CreatureAI.transform.position + direction * _CreatureAI.CreatureData.RetreatDistance +
               (Quaternion.Euler(0, Random.Range(-60, 60), 0) * direction) * 2f;
    }

    public override void Exit()
    {

    }
}