namespace RehvidGames.UI.Player
{
    using Attributes;
    using Interfaces;
    using UnityEngine;
   

    public class PlayerStaminaBar : MonoBehaviour
    {
        [Header("UI Elements")] 
        [SerializeField] private UISlider _slider;

        public void Init(IAttribute attribute)
        {
            _slider.InitSliderValues(attribute.MaxValue, attribute.CurrentValue);
        }
        
        public void UpdateStamina(StaminaAttribute attribute)
        {
            _slider.UpdateSliderValues(attribute.MaxValue, attribute.CurrentValue);
        }
    }
}