//using UnityEngine.UI;
//using UnityEngine;

//public class VolumeSlider : MonoBehaviour
//{
//    [Header("Slider Reference")]
//    [SerializeField] private Slider _slider;

//    private float _volumeStep = 0.1f;
//    private float _minVolume = 0f;
//    private float _maxVolume = 1f;

//    bool bullshit = false;

//    private void Update()
//    {
//        if (bullshit) return;


//        bullshit =true;
//        _slider.value = AudioSystem.Instance.VolumeModifier;
//        PlayerInputHandler.Instance.MoveSliderInputPerformed += VolumeSlider_MoveSliderInputPerformed;
//    }

//    private void VolumeSlider_MoveSliderInputPerformed(int inputValue)
//    {
//        _slider.value = Mathf.Clamp(_slider.value + inputValue * _volumeStep, _minVolume, _maxVolume);
//        AudioSystem.Instance.SetVolumeModifier(_slider.value);
//    }
//}
