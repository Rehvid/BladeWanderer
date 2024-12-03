namespace RehvidGames.Character
{
    using Animator;
    using Attributes;
    using Helpers;
    using Interfaces;
    using ScriptableObjects;
    using UnityEngine;
    using Weapons;

    public abstract class BaseCharacter: MonoBehaviour, ICharacter
    {
        public CharacterEffects CharacterEffects => characterEffects;
        public AnimatorHandler AnimatorHandler => animatorHandler;
        
        [Header("Base configuration")]
        [SerializeField] protected HealthAttribute healthAttribute;
        [SerializeField] protected AnimatorHandler animatorHandler;
        [SerializeField] protected CharacterEffects characterEffects;
        public BaseWeapon Weapon;
        
        protected HitDirectionAnalyzer hitDirectionAnalyzer;
        
        public abstract void ReceiveDamage(float damage, Vector3 hitPosition);
        
        protected void Awake()
        {
            hitDirectionAnalyzer = new HitDirectionAnalyzer();
        }
        
        public bool IsDead() => healthAttribute.IsDead();
    }
}