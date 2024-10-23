using System.Linq;
using UnityEngine;
using System;

public abstract class AttackHandler : MonoBehaviour
{
    [Header("AUDIO SOURCE")]
    [SerializeField] private AudioSource _audioSource;

    // Inherited properties
    protected CreatureAI _creatureAI;
    protected AnimatorManager _AnimatorManager;
    protected AudioSystem _AudioSystem;
    protected AttackData _AttackData;
    protected VFXManager _VFXManager;
    protected GameObject _GameObject;
    protected Transform _Transform;
    protected float _Delta;

    private LayerMask _targetLayer;

    public delegate void SuccessfulAttackDelegate(Collider hit, int colliders, float damage, bool crit);
    public event SuccessfulAttackDelegate SuccessfulAttack;

    public delegate void SuccessfulHitDelegate(Collider hit, float damage, bool crit);
    public event SuccessfulHitDelegate SuccessfulHit;

    public event Action<AttackData> AttackBegun;
    public event Action<AttackData> AttackHalfway;
    public event Action<AttackData> AttackEnded;

    public virtual void Init(LayerMask targetLayer)
    {
        _creatureAI = GetComponent<CreatureAI>();
        _AnimatorManager = GetComponent<AnimatorManager>();
        _AudioSystem = AudioSystem.Instance;
        _VFXManager = VFXManager.Instance;
        _GameObject = gameObject;
        _Transform = transform;

        _targetLayer = targetLayer;
    }

    public void Tick(float delta)
    {
        _Delta = delta;
    }

    public virtual void AttackBegin()
    {
        OnAttackBegun(_AttackData);

        _AudioSystem.PlayAudio(_audioSource, _AttackData.AudioClip, _AttackData.AudioVolume, _AttackData.AudioOffset);
    }

    public virtual void AttackMiddle()
    {
        OnAttackHalfway(_AttackData);

        if (_AttackData.AttackType == AttackType.Special)
        {
            _AttackData.Attack(_creatureAI.CurrentTarget);
        }
        else
        {
            float damage = _AttackData.Damage;
            bool crit = RollCritical();
            HandleDamage(damage, crit);
        }
    }

    public virtual void AttackEnd()
    {
        OnAttackEnded(_AttackData);
    }

    protected void HandleAttack(AttackData attackData)
    {
        if (!_AnimatorManager.GetBool("InAction") && _AnimatorManager.CrossFade(_Delta, attackData.AnimationName))
        {
            _AttackData = attackData;
        }
    }

    private void HandleDamage(float damage, bool crit)
    {
        Collider[] validHits = GetHits().Where(hit => hit.GetComponent<Health>() != null).ToArray();
        if (validHits.Length > 0)
        {
            Collider firstHit = validHits[0];
            int colliders = validHits.Length;

            foreach (Collider hit in validHits)
            {
                if (hit.TryGetComponent<Health>(out var health))
                {
                    damage = HandleCritical(hit, damage, crit);
                    health.TakeDamage(_GameObject, damage);
                    OnsuccesfulHit(hit, damage, crit);
                }
            }

            OnSuccesfulAttack(firstHit, colliders, damage, crit);
        }
    }

    protected virtual float HandleCritical(Collider hit, float damage, bool crit) { return damage; }

    protected virtual bool RollCritical() { return false; }

    private void OnSuccesfulAttack(Collider hit, int colliders, float damage, bool crit)
    {
        SuccessfulAttack?.Invoke(hit, colliders, damage, crit);
    }

    private void OnsuccesfulHit(Collider hit, float damage, bool crit)
    {
        SuccessfulHit?.Invoke(hit, damage, crit);
    }

    private void OnAttackBegun(AttackData attackData)
    {
        AttackBegun?.Invoke(attackData);
    }

    private void OnAttackHalfway(AttackData attackData)
    {
        AttackHalfway?.Invoke(attackData);
    }

    private void OnAttackEnded(AttackData attackData)
    {
        AttackEnded?.Invoke(attackData);
    }

    private Collider[] GetHits()
    {
        Vector3 attackPosition = _Transform.position + _Transform.TransformDirection(_AttackData.AttackOffset);

        if (_AttackData.AttackRadius > 0)
        {
            return Physics.OverlapSphere(attackPosition, _AttackData.AttackRadius, _targetLayer);
        }
        else
        {
            return Physics.OverlapBox(attackPosition, _AttackData.AttackHitBox, _Transform.rotation, _targetLayer);
        }
    }
}