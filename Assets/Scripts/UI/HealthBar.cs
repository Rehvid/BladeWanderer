namespace RehvidGames.UI
{
    using Attributes;
    using Attributes.Base;
    using Interfaces;
    using UnityEngine;
    using UnityEngine.Serialization;

    public class HealthBar: MonoBehaviour
    {
        [Header("UI Elements")] 
        [SerializeField] private UISlider _slider;

        public void Init(BaseAttribute baseAttribute)
        {
            _slider.InitSliderValues(baseAttribute.MaxValue, baseAttribute.CurrentValue);
        }
        
        public void UpdateHealth(Health attribute)
        {
            _slider.UpdateSliderValues(attribute.MaxValue, attribute.CurrentValue);
        }
        
        public void OnDeath(Component sender, object value = null)
        {
            Debug.Log($"Handle death {sender}");
        }
    }
}