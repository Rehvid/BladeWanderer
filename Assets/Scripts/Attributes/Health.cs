namespace RehvidGames.Attributes
{
    using Base;
    using Events.Data;
    using UnityEngine;
    using Interfaces;
    using UI.Slider.Bar;

    public class Health: BaseAttribute, IDamageable
    {
        [SerializeField] private GameEventData _deathTriggered;
        [SerializeField] private AttributeBar _healthBar;
        
        private void Start()
        {
            _healthBar.Init(this);
        }
        
        public void ReceiveDamage(float damage, Vector3 hitPosition)
        {
            CurrentValue -= damage;
            _healthBar.UpdateValues(this);
            if (IsDead())
            {
                _deathTriggered.Raise(this);
            }
        }
        
        public bool IsDead() => CurrentValue <= 0.0f;
    }
}