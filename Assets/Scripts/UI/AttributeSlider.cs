using UnityEngine;
using UnityEngine.UI;

namespace RehvidGames.UI
{
    using Interfaces;

    public class AttributeSlider : MonoBehaviour 
    {
        [SerializeField] private Slider _slider;
        
        public void OnAttributeChanged(IAttribute attribute)
        {
            SetSliderMaxValue(attribute.MaxValue);
            SetSliderValue(attribute.GetInterpolatedOrRawValue());
        }
        
      
        private void SetSliderMaxValue(float newValue)
        {
            _slider.maxValue =  _slider.maxValue >= _slider.value ? newValue : _slider.maxValue;
        }

        private void SetSliderValue(float value)
        {
            _slider.value = Mathf.Clamp(value, 0, _slider.maxValue);
        }
    }
}