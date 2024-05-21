using System.Collections;
using UnityEngine;

public class PlayerMana : Mana
{
    private PlayerDataManager _dataManager;

    [Header("VISUALS")]
    [SerializeField] private GameObject _restorationEffect;

    public delegate void Delegate_MaxManaSet(float maxMana);
    public static event Delegate_MaxManaSet MaxManaSet;

    public delegate void Delegate_CurrentManaChanged(float currentMana);
    public static event Delegate_CurrentManaChanged CurrentManaChanged;

    public delegate void PlayerMana_ManaRestored();
    public static event PlayerMana_ManaRestored ManaRestored;

    private void Awake()
    {
        _dataManager = GetComponent<PlayerDataManager>();
    }

    protected override void ManaInitiliased(float maxMana)
    {
        OnMaxManaSet(maxMana);
    }

    protected override void ManaChanged(float currentMana)
    {
        OnCurrentManaChanged(currentMana);
    }

    private void OnMaxManaSet(float maxMana)
    {
        MaxManaSet?.Invoke(maxMana);
    }

    private void OnCurrentManaChanged(float currentMana)
    {
        CurrentManaChanged?.Invoke(currentMana);
    }

    public void GainManaOverTime(float amount)
    {
        StartCoroutine(GainManaOverTimeCoroutine(amount));
    }

    private IEnumerator GainManaOverTimeCoroutine(float amount)
    {
        GameObject effect = Instantiate(_restorationEffect, _dataManager.GetVFXOrigin());

        float manaRestored = 0;

        while (manaRestored < amount)
        {
            GainMana(1f);

            manaRestored++;

            yield return new WaitForSeconds(0.1f);
        }

        Destroy(effect);

        ManaRestored?.Invoke();
    }
}
