namespace RehvidGames.Attributes
{
    using Data.Events.Events;
    using UnityEngine;
    using Interfaces;
    
    public class HealthAttribute: BaseAttribute, IHealth
    {
        [SerializeField] private GameEvent deathTriggered; 
        
        private void Start()
        {
            InitUI();
        }
        
        public void ReceiveDamage(float damage)
        {
            CurrentValue -= damage;
            StartCoroutine(InterpolateAttributeValue());
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