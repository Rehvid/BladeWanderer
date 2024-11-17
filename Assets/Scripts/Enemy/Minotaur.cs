namespace RehvidGames.Enemy
{
    using Animator;
    using Enums;
    using Managers;
    using UnityEngine;

    public class Minotaur: BaseEnemy
    {
        public override void OnDeath(Component sender, object value = null)
        {
            HandleDeath();
        }

        private void OnEnableCollision()
        {
            Weapon.EnableDamageCollider();
        }

        private void OnDisableCollision()
        {
            Weapon.DisableDamageCollider();
        }

        public override void ReceiveDamage(float damage, Vector3 hitPosition)
        {
            VFXManager.Instance.PlayParticleEffect(CharacterEffects.HitVfx, hitPosition);
            healthAttribute.ReceiveDamage(damage, hitPosition);
            HitDirectionType directionType = hitDirectionAnalyzer.GetFrontBackDirection(hitPosition, transform);
            animatorHandler.SetTrigger(AnimatorParameter.HitDirection);
            animatorHandler.SetTrigger(hitDirectionAnalyzer.GetAnimatorParameterTypeByHitDirectionType(directionType));

            if (hitDirectionAnalyzer.GetDirectionType(hitPosition, transform) != HitDirectionType.Front)
            {
                RotateTowardsToHit(hitPosition);
            }
        }
    }
}