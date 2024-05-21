using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : Health
{
    [Header("REFERENCES")]
    [SerializeField] private GameObject _canvasPrefab;
    [SerializeField] private float yOffset = 4;

    private EnemyHealthUI _enemyHealthUI;
    private GameObject _canvasReference;

    public delegate void EnemyHealth_EnemyDamageTaken(float amount);
    public static event EnemyHealth_EnemyDamageTaken EnemyDamageTaken;

    public delegate void EnemyHealth_EnemyDied();
    public static event EnemyHealth_EnemyDied EnemyDied;

    [Header("DEATH RELATED")]
    [SerializeField] private Animator _animator;
    private NavMeshAgent _agent;
    private Collider _collider;
    private EnemyAI _enemyAI;

    [Header("DAMAGE VFX")]
    [SerializeField] private Transform _origin;
    [SerializeField] private GameObject _bleedEffect;
    private GameObject _currentBleedEffect;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _collider = GetComponent<Collider>();
        _enemyAI = GetComponent<EnemyAI>();

        CreateCanvas();
    }

    private void Update()
    {
        ManageCanvasPosition();
    }

    protected override void HealthInitialised(float maxHealth)
    {
        _enemyHealthUI.UpdateMaxHealth(maxHealth);
    }

    protected override void HealthChanged(float currentHealth)
    {
        _enemyHealthUI.UpdateCurrentHealth(currentHealth);
    }

    protected override void DamageTaken(float amount)
    {
        OnEnemyDamageTaken(amount);
    }

    protected override void Death()
    {
        _animator.SetTrigger("Death");
        _enemyAI.SetIdle();
        _agent.isStopped = true;
        _agent.enabled = false;
        Destroy(_origin.gameObject);
        Destroy(_collider);
        Destroy(_canvasReference);
        OnEnemyDied();
    }

    private void OnEnemyDamageTaken(float amount)
    {
        EnemyDamageTaken?.Invoke(amount);
    }

    private void OnEnemyDied()
    {
        EnemyDied?.Invoke();
    }

    private void CreateCanvas()
    {
        Vector3 position = transform.position + new Vector3(0, yOffset, 0);
        _canvasReference = Instantiate(_canvasPrefab, position, Quaternion.identity);

        _enemyHealthUI = _canvasReference.GetComponent<EnemyHealthUI>();
    }

    private void ManageCanvasPosition()
    {
        if (_canvasReference != null)
        {
            _canvasReference.transform.position = transform.position + new Vector3(0, yOffset, 0);
        }
    }

    public void Bleed(float damage, float duration)
    {
        StopAllCoroutines();
        StartCoroutine(ApplyBleed(damage, duration));
    }

    private IEnumerator ApplyBleed(float damage, float duration)
    {
        if (_currentBleedEffect == null) _currentBleedEffect = Instantiate(_bleedEffect, _origin);

        int ticks = Mathf.CeilToInt(duration / 0.1f);
        float damagePerTick = damage / ticks;

        for (int i = 0; i < ticks; i++)
        {
            yield return new WaitForSeconds(0.1f);

            TakeDamage(damagePerTick);
        }

        Destroy(_currentBleedEffect);
    }
}
