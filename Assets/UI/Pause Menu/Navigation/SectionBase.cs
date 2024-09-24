using UnityEngine;

public abstract class SectionBase : MonoBehaviour
{
    [Header("CONTENT HOLDER REFERENCE")]
    [SerializeField] private GameObject _contentHolder;

    //public virtual void Init(SectionController sectionController)
    //{
    //    _Holder.SetActive(false);
    //}

    //public virtual void Activate()
    //{
    //    _Holder.SetActive(true);
    //}

    //public virtual void Deactivate()
    //{
    //    _Holder.SetActive(false);
    //}
}
