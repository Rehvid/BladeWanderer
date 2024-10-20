namespace RehvidGames.AI
{
    using Enemy;
    using Player;
    using UnityEngine;

    public class AIFight : MonoBehaviour
    {
        [SerializeField] private BaseEnemy _enemy;
        [SerializeField] private Player _player;
        
        [Tooltip("Fight radius in meters")]
        [SerializeField] private float _fightRadius = 2f;

        private bool _isInCombat;
        
        public void OnHitDirectionTaken(Vector3 hitPosition)
        {
            if (!_isInCombat)
            {
                RotateTowardsHit(hitPosition);
            }
        }
        
        private void RotateTowardsHit(Vector3 hitPosition)
        {
            Vector3 directionToHit = (hitPosition - transform.position).normalized;
            directionToHit.y = 0;
            _enemy.transform.rotation = Quaternion.LookRotation(directionToHit);
        }
        
        public void StartAttack()
        {
            _isInCombat = true;
            _enemy.Weapon.SetCurrentlyEquipped(true);
        }

        public void StopAttack()
        {
            _isInCombat = false;
            _enemy?.Weapon.DisableDamageCollider();
        } 

        public bool CanAttack() => Vector3.Distance(transform.position, _player.transform.position) <= _fightRadius;
        
        public bool IsPlayerDead() => _player.IsDead();
    }
}