namespace RehvidGames.Player
{
    using UnityEngine.Events;
    using Attributes;
    using UnityEngine;

    public class PlayerAttributes : MonoBehaviour
    {
        [Header("Attributes")]
        [SerializeField] private HealthAttribute _healthAttribute;
        [SerializeField] private StaminaAttribute _staminaAttribute;
        [SerializeField] private int _currentSouls;
        
        [Header("Events")] 
        [SerializeField] private UnityEvent<int> _soulCounterChanged;
        
        public HealthAttribute HealthAttribute => _healthAttribute;
        public StaminaAttribute StaminaAttribute => _staminaAttribute;
        
        public void AddSouls(int souls)
        {
            _currentSouls += souls;
            _soulCounterChanged?.Invoke(_currentSouls);
        }
    }
}