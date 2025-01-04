namespace RehvidGames.Characters.Player
{
    using DataPersistence.Data.State;
    using Interfaces;
    using RehvidGames.Attributes;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.Serialization;

    public class PlayerAttributes : MonoBehaviour, IDataPersistence<GameState>
    {
        
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
        

        public void LoadData(GameState data)
        {
            PlayerAttributeState attributeState = data.PlayerAttributeState;

            AddSouls(attributeState.CollectedSouls);
            stamina.CurrentValue = attributeState.Stamina.CurrentValue;
            stamina.MaxValue = attributeState.Stamina.MaxValue;
        }

        public void SaveData(GameState data)
        {
            var staminaAttribute = new AttributeState
            {
                CurrentValue = stamina.CurrentValue,
                MaxValue = stamina.MaxValue
            };

            
            data.PlayerAttributeState.Stamina = staminaAttribute;
            data.PlayerAttributeState.CollectedSouls = _currentSouls;
        }
    }
}