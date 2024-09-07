using UnityEngine;

public class RegularDeath : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<Health>().Death += RegularDeath_Death;
    }

    private void OnDisable()
    {
        GetComponent<Health>().Death -= RegularDeath_Death;
    }

    private void RegularDeath_Death(GameObject deathObject)
    {
        Destroy(deathObject);
    }
}
