using System.Collections.Generic;
using UnityEngine;

public class PlayerPotion : MonoBehaviour
{
    private PlayerInputManager _inputManager;

    [Header("POTIONS")]
    [SerializeField] private GameObject[] _potionObjects;
    private List<IPotion> _potions = new List<IPotion>();

    private void Awake()
    {
        _inputManager = GetComponent<PlayerInputManager>();
    }

    private void Start()
    {
        foreach (var potion in _potionObjects)
        {
            _potions.Add(potion.GetComponent<IPotion>());
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
