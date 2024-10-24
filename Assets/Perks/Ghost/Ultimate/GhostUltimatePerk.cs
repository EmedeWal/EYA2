using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

[CreateAssetMenu(fileName = "GhostUltimatePerk", menuName = "Scriptable Object/Perks/Ultimate Perk/Ghost")]
public class GhostUltimatePerk : PerkData
{
    [Header("SPAWN SETTINGS")]
    [SerializeField] private CreatureAI _clonePrefab;
    [SerializeField] private int _cloneCount = 1;
    [SerializeField] private float _minSpawnRadius = 2f;
    [SerializeField] private float _maxSpawnRadius = 3f;

    [Header("CLONE SETTINGS")]
    [SerializeField] private float _attackSpeed = 1;
    [SerializeField] private float _movementSpeed = 1;
    [SerializeField] private float _damageModifier = 1;
    [SerializeField] private float _manaRestoration = 5f;

    [Header("EXPLOSIVE DEATH")]
    [SerializeField] private ExplosionData _deathExplosionData;

    [Header("EXPLOSIVE SUMMON")]
    [SerializeField] private ExplosionData _summonExplosionData;

    [Header("SPARK SETTINGS")]
    [SerializeField] private VFX _sparksVFX;
    [SerializeField] private float _sparksDamage = 10f;
    [SerializeField] private float _sparksRadius = 1f;
    private ConstantAreaDamage _currentSparks;

    private LayerMask _creatureLayer;
    private LayerMask _avoidLayers;

    private List<CreatureAI> _cloneList;

    public override void Init(GameObject playerObject, List<PerkData> perks = null, Dictionary<Stat, float> statChanges = null)
    {
        base.Init(playerObject, perks, statChanges);

        _creatureLayer = LayerMask.GetMask("Controller");
        _avoidLayers = LayerMask.GetMask("DamageCollider", "Controller");
    }

    public override void Activate()
    {
        Transform target = _Lock.Target;
        _cloneList = new List<CreatureAI>();
        _currentSparks = null;

        for (int i = 0; i < _cloneCount; i++)
        {
            CreatureAI currentClone = SpawnClone(target);
            Transform transform = currentClone.transform;
            AddClone(currentClone, target);

            if (_summonExplosionData != null)
            {
                VFX summonVFX = _VFXManager.AddStaticVFX(_summonExplosionData.VFX, transform.position, transform.rotation, 3f);
                summonVFX.GetComponent<Explosion>().InitExplosion(_summonExplosionData.Radius, _summonExplosionData.Damage, _TargetLayer);

                AudioSource source = summonVFX.GetComponent<AudioSource>();
                _AudioSystem.PlayAudio(source, source.clip, source.volume);
            }

            if (_sparksVFX != null)
            {
                _currentSparks = _VFXManager.AddMovingVFX(_sparksVFX, transform).GetComponent<ConstantAreaDamage>();
                _currentSparks.Init(_sparksRadius, _sparksDamage, _TargetLayer);
            }
        }

        _Lock.Locked += GhostUltimatePerk_Locked;
    }

    public override void Tick(float delta)
    {
        for (int i = 0; i < _cloneList.Count; i++)
        {
            CreatureAI currentClone = _cloneList[i];
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

        List<CreatureAI> clonesToRemove = new(_cloneList);
        for (int i = 0; i < clonesToRemove.Count; i++)
        {
            RemoveClone(clonesToRemove[i].gameObject);
        }
    }

    private void AddClone(CreatureAI clone, Transform target)
    {
        clone.GetComponent<AttackHandler>().SuccessfulAttack += GhostUltimatePerk_SuccesfulHit;
        clone.GetComponent<Health>().ValueExhausted += GhostUltimatePerk_ValueExhausted;
        clone.Init(_creatureLayer, _TargetLayer, target);

        clone.AttackHandler.DamageModifier = _damageModifier;
        clone.AnimatorManager.MovementSpeed = _movementSpeed;
        clone.AnimatorManager.AttackSpeed = _attackSpeed;

        _cloneList.Add(clone);
    }

    private void RemoveClone(GameObject cloneObject)
    {
        cloneObject.GetComponent<Health>().ValueExhausted -= GhostUltimatePerk_ValueExhausted;
        CreatureAI clone = cloneObject.GetComponent<CreatureAI>();
        _cloneList.Remove(clone);
        clone.Cleanup();

        Destroy(cloneObject);
    }

    private void GhostUltimatePerk_SuccesfulHit(Collider hit, int colliders, float damage, bool crit)
    {
        _Mana.Gain(_manaRestoration);   
    }

    private void GhostUltimatePerk_Locked(Transform target)
    {
        for (int i = 0; i < _cloneList.Count; i++)
        {
            _cloneList[i].DefaultTarget = target;
        }
    }

    private void GhostUltimatePerk_ValueExhausted(GameObject cloneObject)
    {
        if (_currentSparks != null)
        {
            _VFXManager.RemoveVFX(_currentSparks.GetComponent<VFX>());
        }

        if (_deathExplosionData != null)
        {
            VFX cloneExplosion = _VFXManager.AddStaticVFX(_deathExplosionData.VFX, cloneObject.transform.position, cloneObject.transform.rotation, 3f);
            cloneExplosion.GetComponent<Explosion>().InitExplosion(_deathExplosionData.Radius, _deathExplosionData.Damage, _TargetLayer);
        }

        RemoveClone(cloneObject);
    }

    private CreatureAI SpawnClone(Transform target)
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

        return Instantiate(_clonePrefab, spawnPosition, spawnRotation);
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