namespace RehvidGames.Enemy
{
    using Managers;
    using UnityEngine;

    public class Minotaur: BaseEnemy
    {
        protected override void HandleDeath()
        {
            if (_treasure != null)
            {
                Instantiate(_treasure, transform.position, transform.rotation);
            }
            
            Destroy(gameObject);
        }

        public override void OnDeath(Component sender, object value = null)
        {
            Debug.Log("Minotaur Death");
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
        }
    }
}