using UnityEngine;

public class InvisibleWall : MonoBehaviour
{
    private Collider invisibleWall;

    private void Awake()
    {
        invisibleWall = GetComponent<Collider>();
    }

    private void Start()
    {
        invisibleWall.enabled = false;
    }

    public void Activate()
    {
        invisibleWall.enabled = true;
        Destroy(this);
    }
}
