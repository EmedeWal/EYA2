using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : Health
{
    private CharacterController _characterController;
    private PlayerInputManager _inputManager;
    private PlayerDataManager _dataManager;
    private PlayerMovement _movement;
    private PlayerDash _dash;

    [Header("DEATH RELATED")]
    [SerializeField] private Animator _animator;

    [Header("VISUALS")]
    [SerializeField] private GameObject _restorationEffect;

    public delegate void PlayerHealth_MaxHealthSet(float maxHealth);
    public static event PlayerHealth_MaxHealthSet MaxHealthSet;

    public delegate void PlayerHealth_CurrentHealthChanged(float currentHealth);
    public static event PlayerHealth_CurrentHealthChanged CurrentHealthChanged;

    public delegate void PlayerHealth_PlayerHealed();
    public static event PlayerHealth_PlayerHealed PlayerHealed;

    public delegate void PlayerHealth_PlayerDeath();
    public static event PlayerHealth_PlayerDeath PlayerDeath;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _inputManager = GetComponent<PlayerInputManager>();
        _dataManager = GetComponent<PlayerDataManager>();
        _movement = GetComponent<PlayerMovement>();
    }

    private void OnEnable()
    {
        EnemyHealth.EnemyDamageTaken += PlayerHealth_EnemyDamageTaken;
    }

    private void OnDisable()
    {
        EnemyHealth.EnemyDamageTaken -= PlayerHealth_EnemyDamageTaken;
    }

    protected override void HealthInitialised(float maxHealth)
    {
        OnMaxHealthSet(maxHealth);
    }

    protected override void HealthChanged(float currentHealth)
    {
        OnCurrentHealthChanged(currentHealth);
    }

    protected override void Death()
    {
        _animator.SetTrigger("Death");

        Destroy(_characterController);
        Destroy(_inputManager);
        Destroy(_dataManager.GetVFXOrigin().gameObject);
        Destroy(_movement);
        Destroy(_dash);

        OnPlayerDeath();
    }

    private void PlayerHealth_EnemyDamageTaken(float amount)
    {
        Heal(_dataManager.GetLifeSteal() * amount); 
    }

    private void OnMaxHealthSet(float maxHealth)
    {
        MaxHealthSet?.Invoke(maxHealth);
    }

    private void OnCurrentHealthChanged(float currentHealth)
    {
        CurrentHealthChanged?.Invoke(currentHealth);
    }

    private void OnPlayerDeath()
    {
        PlayerDeath?.Invoke();
    }

    private void OnPlayerHealed()
    {
        PlayerHealed?.Invoke();
    }

    public void HealOverTime(float amount)
    {
        StartCoroutine(HealOverTimeCoroutine(amount));
    }

    private IEnumerator HealOverTimeCoroutine(float amount)
    {
        GameObject effect = Instantiate(_restorationEffect, _dataManager.GetVFXOrigin());

        float healthRestored = 0;

        while (healthRestored < amount)
        {
            Heal(1f);

            healthRestored++;

            yield return new WaitForSeconds(0.1f);
        }

        OnPlayerHealed();

        Destroy(effect);
    }

    public void SetDamageReduction(float damageReduction)
    {
        DamageModifier = 1 - damageReduction;
    }

    public void SetInvincible(bool active)
    {
        Invincible = active;
    }

    private void OnDestroy()
    {
        if (AtMinValue())
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
