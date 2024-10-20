namespace RehvidGames.Enemy
{
    using UnityEngine.Events;
    using Weapons;
    using Attributes;
    using Interfaces;
    using UnityEngine;

    public abstract class BaseEnemy: MonoBehaviour, IDamageable
    {
        [SerializeField] protected HealthAttribute healthAttribute;
        [SerializeField] protected BaseWeapon weapon;
        [SerializeField] protected GameObject _treasure;
        
        [Header("Events")]
        [SerializeField] protected UnityEvent<Vector3> hitDirectionTaken;
        
        public BaseWeapon Weapon => weapon;
        
        public void ReceiveDamage(float damage, Vector3 hitPosition)
        {
            healthAttribute.ReceiveDamage(damage);
            hitDirectionTaken.Invoke(hitPosition);
        }

        protected abstract void HandleDeath();
        public abstract void OnDeath(Component sender, object value = null);
        
        public bool IsDead()
        {
            return healthAttribute.IsDead();
        }
    }
}