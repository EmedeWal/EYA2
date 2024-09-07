using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemySpawner : MonoBehaviour
{
    [Header("SETUP")]
    [SerializeField] private bool _activated = true;
    [SerializeField] private List<Enemy> _enemies = new List<Enemy>();

    [Header("SPAWN LOCATION")]
    [SerializeField] private List<Transform> _portals = new List<Transform>();
    private Vector3 _spawnPosition;
    private Quaternion _spawnRotation;

    [Header("MODIFIERS")]
    [SerializeField] private int _waveValueBase = 5;
    [SerializeField] private int _waveValueModifier = 5;
    [SerializeField] private float _waveDuration = 15f;
    [SerializeField] private float _waveDurationIncrement = 15f;
    private int _waveValue;

    private EnemyManager _enemyManager;

    private List<GameObject> _enemiesToSpawn = new List<GameObject>();
    private float _spawnInterval;

    private List<GameObject> _enemiesSpawned = new List<GameObject>();
    public int _currentWave = 0;

    private int _enemiesKilled = 0;
    private int _enemiesInWave = 0;
    private bool _onCooldown = false;

    public delegate void EnemySpawner_WaveStart(int currentWave);
    public static event EnemySpawner_WaveStart WaveStart;

    public delegate void EnemySpawner_WaveEnd();
    public static event EnemySpawner_WaveEnd WaveEnd;

    private void Awake()
    {
        _enemyManager = GetComponent<EnemyManager>();   
    }

    private void Start()
    {
        if (_activated) StartNextWave();
    }

    private void OnEnable()
    {
        PlayerSkip.Skip += EnemySpawner_Skip;
    }

    private void OnDisable()
    {
        PlayerSkip.Skip -= EnemySpawner_Skip;
    }

    private void EnemySpawner_Skip()
    {
        if (_onCooldown)
        {
            StartNextWave();
        }
    }

    private void EnemySpawner_EnemyHasDied()
    {
        if (!_activated) return;

        _enemiesKilled++;

        if (_enemiesKilled >=  _enemiesInWave)
        {
            _onCooldown = true;
            OnWaveEnd();
        }
    }

    private void StartNextWave()
    {
        _onCooldown = false;
        _enemiesKilled = 0;
        _enemiesInWave = 0;
        _currentWave++;
        OnWaveStart();
        GenerateWave();
    }

    private void GenerateWave()
    {
        _waveDuration += _waveDurationIncrement;
        _waveValue = (_currentWave * _waveValueModifier) + _waveValueBase;

        StartCoroutine(GenerateEnemies());
    }

    private IEnumerator GenerateEnemies()
    {
        List<GameObject> generatedEnemies = new List<GameObject>();

        while (_waveValue > 0)
        {
            int randomEnemyID = Random.Range(0, _enemies.Count);
            int randomEnemyCost = _enemies[randomEnemyID].cost;

            if ((_waveValue >= randomEnemyCost) && (randomEnemyCost <= (_currentWave * 2)))
            {
                generatedEnemies.Add(_enemies[randomEnemyID].enemyPrefab);
                _waveValue -= randomEnemyCost;
            }
            else if (_waveValue <= 0)
            {
                break;
            }

            yield return null;
        }

        _enemiesToSpawn.Clear();
        _enemiesToSpawn = generatedEnemies;
        _spawnInterval = _waveDuration / _enemiesToSpawn.Count;

        _enemiesInWave = _enemiesToSpawn.Count;

        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (_enemiesToSpawn.Count > 0)
        {
            yield return new WaitForSeconds(_spawnInterval);

            GetValidSpawnPoint();
            GameObject currentEnemy = Instantiate(_enemiesToSpawn[0], _spawnPosition, _spawnRotation, transform);

            _enemiesSpawned.Add(currentEnemy);
            currentEnemy.GetComponent<Health>().Death += EnemySpawner_Death;
            _enemyManager.AddEnemy(currentEnemy.GetComponent<Enemy>());
            _enemiesToSpawn.RemoveAt(0);
        }
    }

    private void GetValidSpawnPoint()
    {
        int portal = Random.Range(0, _portals.Count);

        _spawnPosition = _portals[portal].position;
        _spawnRotation = _portals[portal].rotation;
    }

    private void OnWaveStart()
    {
        WaveStart?.Invoke(_currentWave);
    }

    private void OnWaveEnd()
    {
        WaveEnd?.Invoke();
    }

    public void Activate()
    {
        _activated = true;
        StartNextWave();
    }

    private void EnemySpawner_Death(GameObject deathObject)
    {
        _enemiesSpawned.Remove(deathObject);

        if (_enemiesSpawned.Count == 0)
        {
            _onCooldown = true;
            OnWaveEnd();
        }
    }
}
