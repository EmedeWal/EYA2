using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public abstract class FadingUI : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private GameObject _holder;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Image _background;

    [Header("VARIABLES")]
    [SerializeField] private float _displayTime = 3f;
    [SerializeField] private float _fadeTime = 1f;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        SetAlphaUI(0);
        _holder.SetActive(true);
    }

    private void SetAlphaUI(int alpha)
    {
        Color color = _text.color;
        color.a = alpha;
        _text.color = color;
        _background.color = color;
    }

    protected void DisplayMessage()
    {
        _audioSource.Play();

        StopAllCoroutines();
        StartCoroutine(ManageFadeCoroutine());
    }

    private IEnumerator ManageFadeCoroutine()
    {
        yield return StartCoroutine(FadeUICoroutine(0, 1, _fadeTime));
        yield return new WaitForSeconds(_displayTime);
        yield return StartCoroutine(FadeUICoroutine(1, 0, _fadeTime));
    }

    private IEnumerator FadeUICoroutine(float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;
        Color color = _text.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            color.a = alpha;
            _text.color = color;
            _background.color = color;

            yield return null;
        }

        color.a = endAlpha;
        _text.color = color;
        _background.color = color;
    }
}
