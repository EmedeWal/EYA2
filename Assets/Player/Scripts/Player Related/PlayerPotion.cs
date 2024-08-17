using System.Collections.Generic;
using UnityEngine;

public class PlayerPotion : MonoBehaviour
{
    [Header("VARIABLES")]
    [SerializeField] private float _refillBoostOnKill = 5f;

    private PlayerInputManager _inputManager;
    private List<IPotion> _potions = new();

    private void Awake()
    {
        _inputManager = GetComponent<PlayerInputManager>();
    }

    private void Start()
    {
        IPotion[] potions = GetComponentsInChildren<IPotion>();

        foreach (var potion in potions)
        {
            _potions.Add(potion);
        }
    }

    private void OnEnable()
    {
        _inputManager.ConsumePotionInput_Performed += PlayerPotion_ConsumePotionInput_Performed;
        EnemyManager.EnemyDeath += PlayerPotion_EnemyDeath;
    }

    private void OnDisable()
    {
        _inputManager.ConsumePotionInput_Performed -= PlayerPotion_ConsumePotionInput_Performed;
        EnemyManager.EnemyDeath -= PlayerPotion_EnemyDeath;
    }

    private void PlayerPotion_ConsumePotionInput_Performed(float position)
    {
        int intPosition = Mathf.FloorToInt(position);
        if (intPosition > _potions.Count - 1) return;
        _potions[intPosition].Consume();
    }

    private void PlayerPotion_EnemyDeath()
    {
        foreach (var potion in _potions)
        {
            potion.BoostRefill(_refillBoostOnKill);
        }
    }
}
