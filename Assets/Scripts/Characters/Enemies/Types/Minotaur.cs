namespace RehvidGames.Characters.Enemies.Types
{
    using Base;
    using Enums;
    using UnityEngine;
    using VFX;

    public class Minotaur: BaseEnemy
    {
        protected override Collider GetBaseCollider() =>
            TryGetComponent(out CapsuleCollider component) ? component : null;
        
        public override void OnDeath(Component sender, object value = null)
        {
            HandleDeath();
        }

        public override EnemyType GetEnemyType() => EnemyType.Minotaur;
        
        public override void ReceiveDamage(float damage, Vector3 hitPosition)
        {
            health.ReceiveDamage(damage, hitPosition);
            VFXManager.Instance.PlayParticleEffect(enemyEffectsData.HitTakenBloodVfx, hitPosition);

            if (!health.IsDead())
            {
                HandleHitDirection(hitPosition);
            }
        }
        
    }
}