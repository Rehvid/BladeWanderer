namespace RehvidGames.Characters.Enemies.Types
{
    using Base;
    using Enums;
    using Events.Data;
    using Managers;
    using UnityEngine;
    using VFX;

    public class Horseman: BaseEnemy
    {
        public override void ReceiveDamage(float damage, Vector3 hitPosition)
        {
            health.ReceiveDamage(damage, hitPosition);
            VFXManager.Instance.PlayParticleEffect(enemyEffectsData.HitTakenBloodVfx, hitPosition);

            if (!health.IsDead())
            {
                HandleHitDirection(hitPosition);
            }
        }

        public override void OnDeath(Component sender, object value = null)
        {
            if (health.IsDead())
            {
                HandleDeath();
            }
        }

        public override EnemyType GetEnemyType() => EnemyType.Horseman;

        protected override Collider GetBaseCollider() => 
            TryGetComponent(out CapsuleCollider component) ? component : null;
    }
}