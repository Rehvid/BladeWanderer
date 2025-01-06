namespace RehvidGames.Characters.Player
{
    using DataPersistence.Data.State;
    using Managers;
    using Attributes;
    using DataPersistence.Interfaces;
    using UnityEngine;
    using UnityEngine.Events;

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
        
        private void OnDestroy()
        {
            RegistryManager<IDataPersistence<GameState>>.Instance.Unregister(this);
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