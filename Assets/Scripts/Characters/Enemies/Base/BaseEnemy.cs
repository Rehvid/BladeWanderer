namespace RehvidGames.Characters.Enemies.Base
{
    using Characters.Base;
    using Animator;
    using Data;
    using Enemies;
    using Enums;
    using RehvidGames.Items.Weapons.Base;
    using UnityEngine;

    public abstract class BaseEnemy: BaseCharacter
    {
        [Header("Enemy configuration")] 
        [SerializeField] protected BaseWeapon weapon;
        [SerializeField] protected EnemyTreasure _treasure;
        [SerializeField] protected float _deathTime = 5f;
        
        [Header("Enemy character effects")]
        [SerializeField] protected EnemyCharacterEffectsData enemyEffectsData;
        
        public BaseWeapon Weapon => weapon;
        
        public abstract EnemyType GetEnemyType();

        protected abstract Collider GetBaseCollider();

        private void RotateTowardsToHit(Vector3 hitPosition)
        {
            Vector3 directionToHit = (hitPosition - transform.position).normalized;
            directionToHit.y = 0;
            
            transform.rotation = Quaternion.LookRotation(directionToHit);
        }

        protected void HandleHitDirection(Vector3 hitPosition)
        {
            HitDirectionType directionType = hitDirectionAnalyzer.GetFrontBackDirection(hitPosition, transform);
            animatorHandler.SetTrigger(AnimatorParameter.HitDirection);
            animatorHandler.SetInt(AnimatorParameter.HitDirectionType, (int) directionType); 
            
            if (hitDirectionAnalyzer.GetDirectionType(hitPosition, transform) != HitDirectionType.Front)
            {
                RotateTowardsToHit(hitPosition);
            }
        }
        
        protected void HandleDeath()
        {
            animatorHandler.SetTrigger(AnimatorParameter.Death);
            _treasure.CreateTreasureInstance(transform);
            DisableBaseCollider();
            Destroy(gameObject, _deathTime);
        }
        

        private void DisableBaseCollider()
        {
            var baseCollider = GetBaseCollider();
            if (baseCollider)
            {
                baseCollider.enabled = false;
            }
        }
    }
}