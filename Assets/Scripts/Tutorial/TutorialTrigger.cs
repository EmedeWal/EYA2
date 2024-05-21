using UnityEngine;
using UnityEngine.Events;

public class TutorialTrigger : MonoBehaviour
{
    public UnityEvent triggerEnter;

    private void OnTriggerEnter(Collider other)
    {
        triggerEnter.Invoke();
        Destroy(gameObject);
    }
}
