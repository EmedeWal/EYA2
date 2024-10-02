using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

[CreateAssetMenu(fileName = "GhostUltimatePerk", menuName = "Scriptable Object/Perks/Ultimate Perk/Ghost")]
public class GhostUltimatePerk : PerkData
{
    [Header("CLONE DATA")]
    [SerializeField] private CreatureData _creatureData;

    [Header("SPAWN SETTINGS")]
    [SerializeField] private CloneAI _clonePrefab;
    [SerializeField] private int _cloneCount = 1;
    [SerializeField] private float _minSpawnRadius = 2f;
    [SerializeField] private float _maxSpawnRadius = 3f;

    [Header("CLONE SETTINGS")]
    [SerializeField] private VFX _deathExplosionPrefab;
    [SerializeField] private float _deathExplosionRadius = 2f;
    [SerializeField] private float _manaRestoration = 5f;

    [Header("EXPLOSIVE SUMMON")]
    [SerializeField] private VFX _summonExplosionPrefab;
    [SerializeField] private float _summonExplosionRadius = 4f;

    [Header("SPARK SETTINGS")]
    [SerializeField] private ConstantAreaDamage _sparkPrefab;
    [SerializeField] private float _sparksDamage = 10f;
    [SerializeField] private float _sparksRadius = 1f;
    private ConstantAreaDamage _currentSparks;

    private LayerMask _avoidLayers;
    private LayerMask _targetLayer;

    private PlayerLockOn _playerLockOn;
    private Mana _mana;

    private VFXManager _VFXManager;

    private List<CloneAI> _clones;

    public override void Init(List<PerkData> perks, GameObject playerObject)
    {
        base.Init(perks, playerObject);

        for (int i = perks.Count - 1; i >= 0; i--)
        {
            PerkData perk = perks[i];
            if (perk.GetType() == GetType())
            {
                perk.Deactivate();
                perks.RemoveAt(i);
            }
        }

        _avoidLayers = LayerMask.GetMask("DamageCollider", "Controller");
        _targetLayer = LayerMask.GetMask("DamageCollider");

        _playerLockOn = _PlayerObject.GetComponent<PlayerLockOn>();
        _mana = _PlayerObject.GetComponent<Mana>();

        _VFXManager = VFXManager.Instance;

        _clones = new List<CloneAI>();
    }

    public override void Activate()
    {
        Transform target = _playerLockOn.Target;
        _currentSparks = null;

        for (int i = 0; i < _cloneCount; i++)
        {
            Vector3 spawnPosition;
            Quaternion spawnRotation;

            if (target)
            {
                spawnPosition = GetRandomSpawnPosition(target.position);

                Vector3 directionToTarget = target.position - spawnPosition;
                directionToTarget.y = 0;

                spawnRotation = Quaternion.LookRotation(directionToTarget);
            }
            else
            {
                spawnPosition = GetRandomSpawnPosition(_PlayerTransform.position);
                spawnRotation = _PlayerTransform.rotation;
            }

            CloneAI currentClone = Instantiate(_clonePrefab, spawnPosition, spawnRotation);
            currentClone.GetComponent<AttackHandler>().SuccessfulAttack += GhostUltimatePerk_SuccesfulAttack;
            currentClone.GetComponent<Health>().ValueExhausted += GhostUltimatePerk_HealthExhausted;
            currentClone.CreatureData = _creatureData;
            currentClone.Init(_targetLayer);
            _clones.Add(currentClone);

            if (_summonExplosionPrefab != null)
            {
                VFX summonVFX = Instantiate(_summonExplosionPrefab, currentClone.transform);
                _VFXManager.AddVFX(summonVFX, summonVFX.transform);

                Explosion summonExplosion = summonVFX.GetComponent<Explosion>();
                summonExplosion.Init(_summonExplosionRadius, _targetLayer);
            }

            if (_sparkPrefab != null)
            {
                Transform cloneTransform = currentClone.transform;
                _currentSparks = Instantiate(_sparkPrefab, cloneTransform);
                _currentSparks.Init(_sparksRadius, _sparksDamage, _targetLayer);
                VFX sparkVFX = _currentSparks.GetComponent<VFX>();
                _VFXManager.AddVFX(sparkVFX, cloneTransform);
            }

            if (target)
            {
                currentClone.SetChaseTarget(target);
            }
        }

        _playerLockOn.LockedOn += GhostUltimatePerk_LockedOn;
        _mana.ValueExhausted += GhostUltimatePerk_ManaExhausted;

        _mana.RemoveConstantValue(10);
    }

    public override void Tick(float delta)
    {
        for (int i = 0; i < _clones.Count; i++)
        {
            CloneAI currentClone = _clones[i];
            currentClone.Tick(delta);
            currentClone.LateTick(delta);

            if (_currentSparks != null)
            {
                _currentSparks.Tick(delta);
            }
        }
    }

    public override void Deactivate()
    {
        if (_currentSparks != null)
        {
            _VFXManager.RemoveVFX(_currentSparks.GetComponent<VFX>());
        }

        for (int i = 0; i < _clones.Count; i++)
        {
            if (_clones[i] != null)
            {
                RemoveClone(_clones[i]);
            }
        }

        _clones.Clear();
    }

    private void RemoveClone(CloneAI clone)
    {
        clone.GetComponent<Health>().ValueExhausted -= GhostUltimatePerk_HealthExhausted;
        _clones.Remove(clone);
        clone.Cleanup();
    }

    private void GhostUltimatePerk_SuccesfulAttack(Collider hit, float damage, bool crit)
    {
        _mana.GainMana(_manaRestoration);   
    }

    private void GhostUltimatePerk_LockedOn(Transform target)
    {
        if (target)
        {
            for (int i = 0; i < _clones.Count; i++)
            {
                _clones[i].SetChaseTarget(target);
            }
        }
        else
        {
            for (int i = 0; i < _clones.Count; i++)
            {
                _clones[i].CurrentState = CreatureState.Idle;
            }
        }
    }

    private void GhostUltimatePerk_HealthExhausted(GameObject cloneObject)
    {
        if (_currentSparks != null)
        {
            _VFXManager.RemoveVFX(_currentSparks.GetComponent<VFX>());
        }

        if (_deathExplosionPrefab != null)
        {
            VFX cloneExplosion = Instantiate(_deathExplosionPrefab, cloneObject.transform);
            _VFXManager.AddVFX(cloneExplosion, cloneExplosion.transform, true, 3f);
            Explosion explosion = cloneExplosion.GetComponent<Explosion>();
            explosion.Init(_deathExplosionRadius, _targetLayer);
        }

        RemoveClone(cloneObject.GetComponent<CloneAI>());
    }

    private void GhostUltimatePerk_ManaExhausted(GameObject manaObject)
    {
        _mana.ValueExhausted -= GhostUltimatePerk_ManaExhausted;
        Deactivate();
    }

    private Vector3 GetRandomSpawnPosition(Vector3 center)
    {
        Vector3 randomDirection;
        Vector3 finalPosition = center;

        float spawnRadius = _maxSpawnRadius;
        int attemptsPerRadius = 10;
        int maxAttempts = 30;

        for (int i = 0; i < maxAttempts; i++)
        {
            randomDirection = Random.insideUnitSphere * spawnRadius;
            randomDirection += center;
            randomDirection.y = center.y;

            Collider[] nearbyColliders = Physics.OverlapSphere(randomDirection, _minSpawnRadius, _avoidLayers);
            if (nearbyColliders.Length == 0)
            {
                if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, _maxSpawnRadius, NavMesh.AllAreas))
                {
                    finalPosition = hit.position;
                    break;
                }
            }

            if (i > 0 && i % attemptsPerRadius == 0)
            {
                spawnRadius += _minSpawnRadius;
            }
        }

        return finalPosition;
    }

}
