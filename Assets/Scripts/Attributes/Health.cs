namespace RehvidGames.Attributes
{
    using Base;
    using Events.Data;
    using UnityEngine;
    using Interfaces;
    using ScriptableObjects;
    using UI;

    public class Health: BaseAttribute, IDamageable
    {
        [SerializeField] private GameEventData _deathTriggered;
        [SerializeField] private HealthBar _healthBar;
        
        private void Start()
        {
            _healthBar.Init(this);
        }
        
        public void ReceiveDamage(float damage, Vector3 hitPosition)
        {
            CurrentValue -= damage;
            _healthBar.UpdateHealth(this);
            if (IsDead())
            {
                _deathTriggered.Raise(this);
            }
        }
        
        public bool IsDead() => CurrentValue <= 0.0f;
    }
}