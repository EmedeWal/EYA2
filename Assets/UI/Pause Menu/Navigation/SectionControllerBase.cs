using UnityEngine;

public class SectionControllerBase : MonoBehaviour
{

    protected GameObject _Holder;

    public virtual void Init(AudioDataUI audio)
    {
        _Holder = transform.GetChild(0).gameObject;
        _Holder.SetActive(false);
    }

    public virtual void Select()
    {
        _Holder.SetActive(true);
    }

    public virtual void Deselect()
    {
        _Holder.SetActive(false);
    }
}
