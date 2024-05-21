using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class ObjectPositionManager : MonoBehaviour
{
    [Header("OBJECT MOVEMENT")]
    [SerializeField] private Transform moveableObject;
    [SerializeField] private float yOffset = 10f;
    [SerializeField] private float moveDelay = 0.01f;
    [SerializeField] private float moveAmount = 0.1f;

    [Header("AUDIO SETTINGS")]
    [SerializeField] private float audioOffset = 10f;

    private AudioSource audioSource;
    private BoxCollider boxCollider;
    private Vector3 desiredPosition;

    [Header("EVENTS")]
    [SerializeField] private UnityEvent onObjectLowered;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        boxCollider = GetComponent<BoxCollider>();
    }

    public void LowerObject()
    {
        StartCoroutine(LowerObjectFunc());
    }

    private IEnumerator LowerObjectFunc()
    {
        if (audioSource != null) { audioSource.time = audioOffset; audioSource.Play(); }

        desiredPosition = new Vector3(moveableObject.position.x, moveableObject.position.y - yOffset, moveableObject.position.z);

        while (moveableObject.position.y > desiredPosition.y)
        {
            Vector3 newPosition = new Vector3(moveableObject.position.x, moveableObject.position.y - moveAmount, moveableObject.position.z);
            moveableObject.position = newPosition;

            yield return new WaitForSeconds(moveDelay);
        }

        if (boxCollider != null) boxCollider.enabled = false;
        if (audioSource != null) audioSource.Stop();

        onObjectLowered.Invoke();
    }

    public void RaiseObject()
    {
        StartCoroutine(RaiseObjectFunc());
    }

    private IEnumerator RaiseObjectFunc()
    {
        if (boxCollider != null) boxCollider.enabled = true;

        desiredPosition = new Vector3(moveableObject.position.x, moveableObject.position.y + yOffset, moveableObject.position.z);

        while (moveableObject.position.y < desiredPosition.y)
        {
            Vector3 newPosition = new Vector3(moveableObject.position.x, moveableObject.position.y + moveAmount, moveableObject.position.z);
            moveableObject.position = newPosition;

            yield return new WaitForSeconds(moveDelay);
        }
    }
}
