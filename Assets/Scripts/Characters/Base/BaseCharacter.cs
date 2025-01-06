namespace RehvidGames.Characters.Base
{
    using Animator;
    using Attributes;
    using Interfaces;
    using UnityEngine;
    using Utilities;

    public abstract class BaseCharacter: MonoBehaviour, IDamageable
    {
        public AnimatorHandler AnimatorHandler => animatorHandler;
        
        [Header("Base configuration")]
        [SerializeField] protected Health health;
        [SerializeField] protected AnimatorHandler animatorHandler;
        
        protected HitDirectionAnalyzer hitDirectionAnalyzer;
        
        public abstract void ReceiveDamage(float damage, Vector3 hitPosition);
        
        public abstract void OnDeath(Component sender, object value = null);
        
        public virtual bool IsDead() => health.IsDead();
        
        protected virtual void Awake()
        {
            hitDirectionAnalyzer = new HitDirectionAnalyzer();
        }
    }
}