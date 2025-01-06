namespace RehvidGames.UI.Slider.Bar.Base
{
    using RehvidGames.Attributes.Base;
    using Slider;
    using UnityEngine;

    public abstract class BaseAttributeBar: MonoBehaviour
    {
        [Header("Slider Manager")] 
        [SerializeField] protected UISliderManager sliderManager;

        public abstract void Init(BaseAttribute baseAttribute);
        public abstract void UpdateValues(BaseAttribute baseAttribute);
    }
}