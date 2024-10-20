namespace RehvidGames.Enemy
{
    using UnityEngine;

    public class Skeleton: BaseEnemy
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
            Debug.Log("Skeleton Death");
        }

        private void OnEnableCollision()
        {
            weapon.EnableDamageCollider();
        }

        private void OnDisableCollision()
        {
            weapon.DisableDamageCollider();
        }
    }
}