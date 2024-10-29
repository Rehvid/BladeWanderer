namespace RehvidGames.Character
{
    using Animator;
    using Attributes;
    using Enums;
    using Helpers;
    using Interfaces;
    using ScriptableObjects;
    using UnityEngine;
    using Weapons;

    public abstract class BaseCharacter: MonoBehaviour, ICharacter
    {
        public BaseWeapon Weapon;
        public CharacterEffects CharacterEffects => characterEffects;
        
        [SerializeField] protected HealthAttribute healthAttribute;
        [SerializeField] protected AnimatorController _animatorController;
        [SerializeField] protected CharacterEffects characterEffects;
        
        protected HitDirectionAnalyzer hitDirectionAnalyzer;
        
        public abstract void ReceiveDamage(float damage, Vector3 hitPosition);
        
        private void Awake()
        {
            hitDirectionAnalyzer = new HitDirectionAnalyzer();
        }
        
        public bool IsDead() => healthAttribute.IsDead();
    }
}