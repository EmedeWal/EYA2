using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class CreatureManager : MonoBehaviour
{
    private float _delta;

    [Header("CREATURE PREFABS")]
    [SerializeField] private List<CreatureAI> _creaturePrefabList = new();
    private List<CreatureAI> _activeCreatureList = new();

    private LayerMask _creatureLayer;
    private LayerMask _targetLayer;

    public static event Action<CreatureAI> CreatureDeath;

    public void Init()
    {
        _creatureLayer = LayerMask.GetMask("DamageCollider");
        _targetLayer = LayerMask.GetMask("Controller");

        CollectCreatures();
    }

    public void Tick(float delta)
    {
        _delta = delta;

        for (int i = 0; i < _activeCreatureList.Count; i++)
        {
            _activeCreatureList[i].Tick(_delta);
        }
    }

    public void LateTick(float delta)
    {
        _delta = delta;

        for (int i = 0; i < _activeCreatureList.Count; i++)
        {
            _activeCreatureList[i].LateTick(_delta);
        }
    }

    private void AddCreature(CreatureAI creature)
    {
        _activeCreatureList.Add(creature);
        creature.Init(_creatureLayer, _targetLayer);
        creature.Health.ValueExhausted += CreatureManager_ValueExhausted;
    }

    private void RemoveCreature(GameObject creatureObject)
    {
        CreatureAI creature = creatureObject.GetComponent<CreatureAI>();
        creature.Health.ValueExhausted -= CreatureManager_ValueExhausted;
        creature.AnimatorManager.CrossFadeAnimation(_delta, "Die");
        _activeCreatureList.Remove(creature);
        OnCreatureDeath(creature);

        float delay = creature.AnimatorManager.GetAnimationLength();
        StartCoroutine(DestroyCreatureObject(creatureObject, delay));

        Destroy(creature.LockTarget);
        Destroy(creature.Health);
        Destroy(creature);
    }

    private void CreatureManager_ValueExhausted(GameObject creatureObject)
    {
        RemoveCreature(creatureObject);
    }

    private void OnCreatureDeath(CreatureAI creature)
    {
        CreatureDeath?.Invoke(creature);
    }

    private void CollectCreatures()
    {
        CreatureAI[] creatureAIArray = FindObjectsByType<CreatureAI>(FindObjectsSortMode.None);
        foreach (CreatureAI creatureAI in creatureAIArray) AddCreature(creatureAI);
    }

    private IEnumerator DestroyCreatureObject(GameObject creatureObject, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(creatureObject);
    }
}