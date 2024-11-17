namespace RehvidGames.Enemy
{
    using Animator;
    using Character;
    using UnityEngine;

    public abstract class BaseEnemy: BaseCharacter
    {
        [Header("Enemy configuration")]
        [SerializeField] protected GameObject _treasure;
        [SerializeField] protected float _deathTime = 5f;
        
        public abstract void OnDeath(Component sender, object value = null);

        protected virtual Collider GetCollider() => TryGetComponent(out CapsuleCollider component) ? component : null;

        protected virtual void RotateTowardsToHit(Vector3 hitPosition)
        {
            Vector3 directionToHit = (hitPosition - transform.position).normalized;
            directionToHit.y = 0;
            transform.rotation = Quaternion.LookRotation(directionToHit);
        }
        
        protected virtual void HandleDeath()
        {
            animatorHandler.SetTrigger(AnimatorParameter.Death);
            DisableBaseCollider();
            CreateTreasureInstance();
            Destroy(gameObject, _deathTime);
        }

        private void DisableBaseCollider()
        {
            var baseCollider = GetCollider();
            if (baseCollider)
            {
                baseCollider.enabled = false;
            }
        }
        
        private void CreateTreasureInstance()
        {
            if (_treasure == null) return;
            var treasureInstance = Instantiate(_treasure, transform.position + Vector3.up, transform.rotation);
            treasureInstance.transform.SetParent(null); 
        }
        
    }
}