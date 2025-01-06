namespace RehvidGames.Attributes
{
    using Base;
    using UI.Slider.Bar;
    using UnityEngine;

    public class Stamina: BaseAttribute
    {
        [SerializeField] private AttributeBar _staminaBar;
        [SerializeField] private AttributeRegenerator _regenerator;
        
        private void Start()
        {
            _staminaBar.Init(this);
        }
        
        public void UseStamina(float useStamina)
        {
            if (!HasEnoughStamina(useStamina)) return;
            CurrentValue -= useStamina;
            _staminaBar.UpdateValues(this);
        }

        public void Regeneration()
        {
            if (_regenerator.IsRegenerationStarted()) return;
            _regenerator.StartRegeneration(
                this, 
                (_) => _staminaBar.UpdateValues(this)
            );
        }

        public bool isRegenerationStarted() => _regenerator.IsRegenerationStarted();
        
        public void StopRegeneration() => _regenerator.StopRegeneration();
        
        public bool HasEnoughStamina(float stamina) => CurrentValue - stamina > 0;
    }
}