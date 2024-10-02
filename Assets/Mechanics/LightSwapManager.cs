using System.Collections;
using UnityEngine;

public class LightSwapManager : MonoBehaviour
{
    [SerializeField] private Color[] _colors;
    [SerializeField] private float _transitionTime = 2f;
    [SerializeField] private float _waitTime = 2f;
    private Light _lightSource;
    private int _colorIndex;

    private void Awake()
    {
        _lightSource = GetComponent<Light>();

        _colorIndex = Random.Range(0, _colors.Length);
        _lightSource.color = _colors[_colorIndex];

        StartCoroutine(SwitchColor());
    }

    private IEnumerator SwitchColor()
    {
        while (true)
        {
            Color startColor = _lightSource.color;
            Color endColor = _colors[_colorIndex];

            float time = 0;

            while (time < _transitionTime)
            {
                _lightSource.color = Color.Lerp(startColor, endColor, time / _transitionTime);
                time += Time.deltaTime;
                yield return null;
            }

            _lightSource.color = endColor;

            yield return new WaitForSeconds(_waitTime);

            _colorIndex = (_colorIndex + 1) % _colors.Length;
        }
    }
}
