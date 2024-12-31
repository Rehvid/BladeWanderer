namespace RehvidGames.UI.Player
{
    using Attributes;
    using Attributes.Base;
    using Interfaces;
    using UnityEngine;
   

    public class PlayerStaminaBar : MonoBehaviour
    {
        [Header("UI Elements")] 
        [SerializeField] private UISlider _slider;

        public void Init(BaseAttribute baseAttribute)
        {
            _slider.InitSliderValues(baseAttribute.MaxValue, baseAttribute.CurrentValue);
        }
        
        public void UpdateStamina(Stamina attribute)
        {
            _slider.UpdateSliderValues(attribute.MaxValue, attribute.CurrentValue);
        }
    }
}