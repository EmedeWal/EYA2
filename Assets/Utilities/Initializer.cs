using UnityEngine;

public class Initializer : MonoBehaviour
{
    [Header("VALUES")]
    [SerializeField] private float _maxValue = 100f;
    [SerializeField] private float _currentValue = 75f;

    private void Start()
    {
        GetComponent<Health>().Init(_maxValue, _currentValue);

        Mana mana = GetComponent<Mana>();

        if (mana != null)
        {
            mana.Init(_maxValue, _currentValue);
        }
    }
}
