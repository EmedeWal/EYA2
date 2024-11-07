using System.Collections.Generic;
using UnityEngine;

namespace EmeWillem
{
    namespace AI
    {
        public class Enemy : MonoBehaviour
        {
            [HideInInspector] public Transform DefaultTarget;
            [HideInInspector] public Transform CurrentTarget;

            [Header("REFERENCES")]
            [SerializeField] private LockTarget _lockTarget;
            [SerializeField] private EnemyData _enemyData;

            [Header("COMPONENTS")]
            [SerializeField] private List<DefenseCollider> _defenseColliderList = new();
            [SerializeField] private List<OffenseCollider> _offenseColliderList = new();

            public EnemyData EnemyData => _enemyData;
            public AnimatorManager AnimatorManager { get; private set; }
            public AttackHandler AttackHandler { get; private set; }
            public Locomotion Locomotion { get; private set; }
            public Transform Transform { get; private set; }
            public LayerMask TargetLayer { get; private set; }

            private EnemyState _currentState;
            private Health _health;
            private Posture _posture;

            private void Awake()
            {
                Init(LayerMask.GetMask("DamageCollider"), LayerMask.GetMask("Controller"), GameObject.FindGameObjectWithTag("Player").transform);
            }

            private void Update()
            {
                Tick(Time.deltaTime);
            }

            private void LateUpdate()
            {
                LateTick();
            }

            private void OnDisable()
            {
                Cleanup();
            }

            public virtual void Init(LayerMask enemyLayer, LayerMask targetLayer, Transform defaultTarget)
            {
                Transform = transform;

                DefaultTarget = defaultTarget;
                TargetLayer = targetLayer;

                AnimatorManager = GetComponent<AnimatorManager>();
                AttackHandler = GetComponent<AttackHandler>();
                Locomotion = GetComponent<Locomotion>();
                _health = GetComponent<Health>();
                _posture = GetComponent<Posture>();

                AnimatorManager.Init();
                AttackHandler.Init(_offenseColliderList);
                Locomotion.Init(_enemyData.WalkSpeed, _enemyData.RunSpeed);
                _health.Init(_enemyData.Health);
                _posture.Init(_enemyData.Posture, _enemyData.PostureRecovery);
                _lockTarget.Init(_health, _enemyData.LockPointOffset);

                foreach (DefenseCollider defenseCollider in _defenseColliderList)
                {
                    defenseCollider.Init(gameObject, _health, _posture, enemyLayer);
                }

                foreach (OffenseCollider offenseCollider in _offenseColliderList)
                {
                    offenseCollider.Init(Transform, TargetLayer);
                }

                SetState(new IdleState(this));
            }

            public virtual void Tick(float delta)
            {
                float locomotionValue = Locomotion.CalculateLocomotionValue();
                AnimatorManager.Tick(delta, locomotionValue);
                _currentState?.Tick(delta);
            }

            public virtual void LateTick()
            {
                _health.LateTick();
                _posture.LateTick();
            }

            public virtual void Cleanup()
            {
                Destroy(Locomotion);
                Destroy(_health);
                Destroy(_posture);
                Destroy(_lockTarget);
            }

            public virtual void DetermineBehavior(BaseAttackData attackData, Transform target)
            {
                DetermineAttack(target);
            }

            public virtual void DetermineAttack(Transform target)
            {
                CurrentTarget = target;

                List<AttackData> viableAttacks = new();

                if (IsTargetInFront(CurrentTarget))
                {
                    viableAttacks.AddRange(AttackHandler.AttackDataList);
                }
                else
                {
                    viableAttacks.AddRange(AttackHandler.AttackDataList.FindAll(a => a.RotationSpeed > 0));
                }

                AttackHandler.SelectRandomAttack(viableAttacks);
            }

            public void SetState(EnemyState newState)
            {
                _currentState?.Exit();
                _currentState = newState;
                _currentState.Enter();
            }

            public Transform GetNearestTarget(float maxDistance)
            {
                Collider[] hitColliders = Physics.OverlapSphere(Transform.position, maxDistance, TargetLayer);
                Transform nearestTarget = null;
                float shortestDistance = Mathf.Infinity;

                if (hitColliders.Length > 0)
                {
                    foreach (Collider hitCollider in hitColliders)
                    {
                        float distance = Vector3.Distance(Transform.position, hitCollider.transform.position);

                        if (distance < shortestDistance)
                        {
                            shortestDistance = distance;
                            nearestTarget = hitCollider.transform;
                        }
                    }
                }

                return nearestTarget;
            }

            public float GetDistanceToTarget(Transform target)
            {
                return Vector3.Distance(Transform.position, target.position);
            }

            public bool IsTargetInRange(Transform target)
            {
                float threshold = _enemyData.AttackDistance + AttackHandler.AttackData.DistanceUnits;
                float distance = GetDistanceToTarget(target);
                return distance <= threshold;
            }

            public bool IsTargetInFront(Transform target)
            {
                Vector3 directionToTarget = (target.position - Transform.position).normalized;
                float angleToTarget = Vector3.Angle(Transform.forward, directionToTarget);
                return angleToTarget < _enemyData.MaxAngle;
            }

            public bool IsTargetBehind(Transform target)
            {
                Vector3 directionToTarget = (target.position - Transform.position).normalized;
                float angleToTarget = Vector3.Angle(Transform.forward, directionToTarget);
                return angleToTarget > 90;
            }
        }
    }
}