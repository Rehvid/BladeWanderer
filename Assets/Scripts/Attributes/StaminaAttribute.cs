namespace RehvidGames.Attributes
{
    using UI.Player;
    using UnityEngine;

    public class StaminaAttribute: BaseAttribute
    {
        [SerializeField] private PlayerStaminaBar _staminaBar;
        [SerializeField] private AttributeRegenerator _regenerator;
        
        private void Start()
        {
            _staminaBar.Init(this);
        }
        
        public void UseStamina(float useStamina)
        {
            if (!HasEnoughStamina(useStamina)) return;
            CurrentValue -= useStamina;
            _staminaBar.UpdateStamina(this);
        }

        public void Regeneration()
        {
            if (_regenerator.IsRegenerationStarted()) return;
            _regenerator.StartRegeneration(
                this, 
                (_) => _staminaBar.UpdateStamina(this)
            );
        }

        public bool isRegenerationStarted() => _regenerator.IsRegenerationStarted();
        
        public void StopRegeneration() => _regenerator.StopRegeneration();
        
        public bool HasEnoughStamina(float stamina) => CurrentValue - stamina > 0;
    }
}