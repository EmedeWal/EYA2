using UnityEngine;
using TMPro;

public class WaveUI : MonoBehaviour
{
    [SerializeField] private GameObject _holder;
    [SerializeField] private TextMeshProUGUI _waveText;

    private void Start()
    {
        ManageHolder(false);
        ManageText(false);
    }

    private void OnEnable()
    {
        EnemySpawner.WaveStart += WaveUI_WaveStart;
        EnemySpawner.WaveEnd += WaveUI_WaveEnd;
    }

    private void OnDisable()
    {
        EnemySpawner.WaveStart -= WaveUI_WaveStart;
        EnemySpawner.WaveEnd += WaveUI_WaveEnd;
    }

    private void WaveUI_WaveStart(int currentWave)
    {
        ManageHolder(false);
        ManageText(true);

        _waveText.text = $"Wave: {currentWave}";
    }

    private void WaveUI_WaveEnd()
    {
        ManageHolder(true);
    }

    private void ManageText(bool active)
    {
        _waveText.gameObject.SetActive(active);
    }

    private void ManageHolder(bool active)
    {
        _holder.SetActive(active);
    }
}
