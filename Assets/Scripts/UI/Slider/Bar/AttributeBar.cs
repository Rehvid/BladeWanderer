namespace RehvidGames.UI.Slider.Bar
{
    using Attributes.Base;
    using Base;

    public class AttributeBar: BaseAttributeBar
    {
        public override void Init(BaseAttribute baseAttribute)
        {
            sliderManager?.InitSliderValues(baseAttribute.MaxValue, baseAttribute.CurrentValue);
        }

        public override void UpdateValues(BaseAttribute baseAttribute)
        {
            sliderManager?.UpdateSliderValues(baseAttribute.MaxValue, baseAttribute.CurrentValue);
        }
    }
}