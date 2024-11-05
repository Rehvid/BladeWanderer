using UnityEngine;
using UnityEngine.UI;

namespace RehvidGames.UI
{
    using System.Collections;
    using DG.Tweening;
    using Interfaces;

    public class UISlider : MonoBehaviour 
    {
        [Header("Slider configuration")]
        [SerializeField] private Slider _slider;
        [SerializeField] private float _dotweenDuration = 0.5f;
        
        
        public void InitSliderValues(float maxValue, float currentValue)
        {
            SetSliderMaxValue(maxValue);
            SetSliderValue(currentValue);
        }

        public void UpdateSliderValues(float maxValue, float currentValue)
        {
            DOTween.To(
                () => _slider.maxValue,
                SetSliderMaxValue,
                maxValue,
                _dotweenDuration
            );
            DOTween.To(
                    () => _slider.value, 
                    SetSliderValue,
                    currentValue,
                    _dotweenDuration
            ).SetEase(Ease.OutCubic);
        }
        
        private void SetSliderMaxValue(float newValue) => _slider.maxValue =  _slider.maxValue >= _slider.value ? newValue : _slider.maxValue;
        
        private void SetSliderValue(float value) => _slider.value = Mathf.Clamp(value, 0, _slider.maxValue);
    }
}