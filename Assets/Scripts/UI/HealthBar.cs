namespace RehvidGames.UI
{
    using Attributes;
    using Interfaces;
    using UnityEngine;
    using UnityEngine.Serialization;

    public class HealthBar: MonoBehaviour
    {
        [Header("UI Elements")] 
        [SerializeField] private UISlider _slider;

        public void Init(IAttribute attribute)
        {
            _slider.InitSliderValues(attribute.MaxValue, attribute.CurrentValue);
        }
        
        public void UpdateHealth(HealthAttribute attribute)
        {
            _slider.UpdateSliderValues(attribute.MaxValue, attribute.CurrentValue);
        }
        
        public void OnDeath(Component sender, object value = null)
        {
            Debug.Log($"Handle death {sender}");
        }
    }
}