using UnityEngine;

public class Section : MonoBehaviour
{
    public bool IsUnlocked { get; private set; }

    [SerializeField] private GameObject _gameObject;

    public void Init()
    {
        _gameObject.SetActive(false);
        IsUnlocked = true;
    }

    public void Activate()
    {
        if (IsUnlocked)
        {
            _gameObject.SetActive(true);
        }
    }

    public void Deactivate()
    {
        _gameObject.SetActive(false);
    }

    public void Unlock()
    {
        IsUnlocked = true;
    }
}
