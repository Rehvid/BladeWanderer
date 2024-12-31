namespace RehvidGames.Characters.Player
{
    using RehvidGames.Attributes;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.Serialization;

    public class PlayerAttributes : MonoBehaviour
    {
        [FormerlySerializedAs("_staminaAttribute")]
        [Header("Attributes")]
        [SerializeField] private Stamina stamina;
        [SerializeField] private int _currentSouls;
        
        [Header("Events")] 
        [SerializeField] private UnityEvent<int> _soulCounterChanged;
        
        public Stamina Stamina => stamina;
        
        public int CurrentSouls => _currentSouls;
        
        public void AddSouls(int souls)
        {
            _currentSouls += souls;
            _soulCounterChanged?.Invoke(_currentSouls);
        }
        
    }
}