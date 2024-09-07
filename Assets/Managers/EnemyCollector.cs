using System.Collections.Generic;
using UnityEngine;

public class EnemyCollector : MonoBehaviour
{
    [SerializeField] private List<Enemy> _enemyList = new();
    private EnemyManager _enemyManager;

    private void Awake()
    {
        _enemyManager = GetComponent<EnemyManager>();
    }

    private void Start()
    {
        foreach (Enemy enemy in _enemyList)
        {
            _enemyManager.AddEnemy(enemy);
        }
    }
}
