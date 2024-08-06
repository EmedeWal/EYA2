using System.Collections.Generic;
using UnityEngine;

public class PlayerPotion : MonoBehaviour
{
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
    }

    private void OnDisable()
    {
        _inputManager.ConsumePotionInput_Performed -= PlayerPotion_ConsumePotionInput_Performed;
    }

    private void PlayerPotion_ConsumePotionInput_Performed(float position)
    {
        int intPosition = Mathf.FloorToInt(position);

        if (intPosition > _potions.Count - 1) return;

        _potions[intPosition].Consume();
    }
}
