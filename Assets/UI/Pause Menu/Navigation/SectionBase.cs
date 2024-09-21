using UnityEngine;

public abstract class SectionBase : MonoBehaviour
{
    [Header("CONTENT HOLDER REFERENCE")]
    [SerializeField] private GameObject _contentHolder;

    public virtual void Init(SectionController sectionController)
    {
        _contentHolder.SetActive(false);
    }

    public virtual void Activate()
    {
        _contentHolder.SetActive(true);
    }

    public virtual void Deactivate()
    {
        _contentHolder.SetActive(false);
    }
}
