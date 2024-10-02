using UnityEngine.AI;
using UnityEngine;

public class CreatureAI : MonoBehaviour
{
    [Header("CREATURE BASE DATA")]
    [SerializeField] private CreatureData _creatureData;

    private NavMeshAgent _navMeshAgent;

    private CreatureState _creatureState;

    public void Init()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public void Tick(float delta)
    {

    }

    public void Cleanup()
    {
        Destroy(gameObject);
    }
}
