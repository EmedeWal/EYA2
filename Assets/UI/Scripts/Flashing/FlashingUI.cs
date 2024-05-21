using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FlashingUI : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private float _flashTime;

    private void Start()
    {
        StartCoroutine(FlashCoroutine());
    }

    private IEnumerator FlashCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_flashTime);

            if (_icon.gameObject.activeSelf)
            {
                _icon.gameObject.SetActive(false);
            }
            else
            {
                _icon.gameObject.SetActive(true);
            }
        }
    }
}
