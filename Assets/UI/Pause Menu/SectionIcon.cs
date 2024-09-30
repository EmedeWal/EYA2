using UnityEngine;

public class SectionIcon : MonoBehaviour
{
    private GameObject _glowObject;

    public void Init()
    {
        _glowObject = transform.GetChild(0).gameObject;
        _glowObject.SetActive(false);
    }

    public void Select()
    {
        _glowObject.SetActive(true);
    }

    public void Deselect()
    {
        _glowObject.SetActive(false);
    }
}
