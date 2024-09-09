using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class EnemyManager : MonoBehaviour
{
    [Header("ENEMY MODEL CLEAN UP CONTAINER")]
    [SerializeField] private GameObject _cleanupContainer;

    private List<Enemy> _enemies = new();

    // Events
    public static event Action EnemyDeath;

    public void AddEnemy(Enemy enemy)
    {
        Health health = enemy.GetComponent<Health>();
        health.Death += EnemyManager_Death;
        health.Initialize();
        _enemies.Add(enemy);
    }

    public void RemoveEnemy(Enemy enemy)
    {
        Health health = enemy.GetComponent<Health>();
        health.Death -= EnemyManager_Death;
        _enemies.Remove(enemy);
    }

    private IEnumerator HandleEnemyDeath(GameObject enemyObject)
    {
        Transform enemyGFX = enemyObject.transform.GetChild(0);
        enemyGFX.SetParent(_cleanupContainer.transform);
        RemoveEnemy(enemyObject.GetComponent<Enemy>());
        Destroy(enemyObject);

        if (enemyGFX.TryGetComponent<Animator>(out var animator))
        {
            animator.SetTrigger("Death");
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        }

        Destroy(enemyGFX.gameObject);
    }

    private void EnemyManager_Death(GameObject enemyObject)
    {
        OnEnemyDeath();
        StartCoroutine(HandleEnemyDeath(enemyObject));
    }

    private void OnEnemyDeath()
    {
        EnemyDeath?.Invoke();
    }
}
