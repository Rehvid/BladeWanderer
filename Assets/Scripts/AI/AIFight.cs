namespace RehvidGames.AI
{
    using Animator;
    using Enemy;
    using Player;
    using UnityEngine;
    using Weapons;

    public class AIFight : MonoBehaviour
    {
        public bool IsAttacking { get; private set; }

        [SerializeField] private BaseEnemy _enemy;
        [SerializeField] private Player _player;
        
        [Tooltip("Fight radius in meters")]
        [SerializeField] private float _fightRadius = 2f;

        private BaseWeapon _weapon;
        
        private void Start()
        {
            if (!_enemy) return;
            _weapon = _enemy.Weapon;
            _weapon.SetCurrentlyEquipped(true);
        }

        public void OnStartAttack()
        {
            _weapon?.EnableDamageCollider();
        }

        public void OnStopAttack()
        {
            _weapon?.DisableDamageCollider();
        }

        public void OnEndAttack()
        {
            IsAttacking = false;
        }
        
        public void Attack()
        {
            if (!CanAttack() || IsAttacking) return;
            _enemy.AnimatorHandler.SetTrigger(AnimatorParameter.Attack);
            IsAttacking = true;
        }
        
        public bool CanAttack() => Vector3.Distance(transform.position, _player.transform.position) <= _fightRadius && !IsPlayerDead();
        
        public bool IsPlayerDead() => _player.IsDead();
    }
}