namespace RehvidGames.Player
{
    using UnityEngine.Events;
    using Attributes;
    using UnityEngine;

    public class PlayerAttributes : MonoBehaviour
    {
        [Header("Attributes")]
        [SerializeField] private StaminaAttribute _staminaAttribute;
        [SerializeField] private int _currentSouls;
        
        [Header("Events")] 
        [SerializeField] private UnityEvent<int> _soulCounterChanged;
        
        public StaminaAttribute Stamina => _staminaAttribute;
        
        public void AddSouls(int souls)
        {
            _currentSouls += souls;
            _soulCounterChanged?.Invoke(_currentSouls);
        }
    }
}