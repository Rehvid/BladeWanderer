namespace RehvidGames.Attributes
{
    using UnityEngine;
    using Interfaces;
    using ScriptableObjects;
    using UI;

    public class HealthAttribute: BaseAttribute, IHealth
    {
        [SerializeField] private GameEvent deathTriggered;
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
                deathTriggered.Raise(this);
            }
        }
        
        public bool IsDead()
        {
            return CurrentValue <= 0.0f;
        }
    }
}