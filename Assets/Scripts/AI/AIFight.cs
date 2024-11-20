namespace RehvidGames.AI
{
    using Animator;
    using Enemy;
    using Enums;
    using Player;
    using UnityEngine;
    using Weapons;

    public class AIFight : MonoBehaviour
    {
        public AttackStateType CurrentAttackState { get; private set; } = AttackStateType.Ready;
        
        [SerializeField] private BaseEnemy _enemy;
        [SerializeField] private Player _player;
        
        [Tooltip("Fight radius in meters")]
        [SerializeField] private float _fightRadius = 2f;

        private BaseWeapon _weapon;
        private AnimatorHandler _animatorHandler;
        
        
        private void Start()
        {
            if (!_enemy) return;
            _weapon = _enemy.Weapon;
            _weapon.SetCurrentlyEquipped(true);
            _animatorHandler = _enemy.AnimatorHandler;
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
            CurrentAttackState = AttackStateType.Ready;
        }

        public void SetReadyAttackState() => CurrentAttackState = AttackStateType.Ready;
        
        public void Attack()
        {
            _animatorHandler.SetTrigger(AnimatorParameter.Attack);
            CurrentAttackState = AttackStateType.Attack;
        }

        public void ResetAttack()
        {
            CurrentAttackState = AttackStateType.WaitingForAnimation;
            Debug.Log($"Transitioning to: {CurrentAttackState}");
            StartCoroutine(
                _animatorHandler.WaitForCurrentAnimationThenInvoke(() =>
                {
                    _animatorHandler.SetTrigger(AnimatorParameter.Attack);
                    _animatorHandler.SetInt(AnimatorParameter.HitDirectionType, 0);
                    CurrentAttackState = AttackStateType.Attack;
                    Debug.Log($"Animation finished. State: {CurrentAttackState}");
                })
            );
        }
        
        public bool CanAttack() => 
            Vector3.Distance(transform.position, _player.transform.position) <= _fightRadius 
            && !IsPlayerDead() 
            && CurrentAttackState == AttackStateType.Ready;

        public bool CanResetAttack() =>
            CurrentAttackState == AttackStateType.Attack && !IsDirectionTypeAnimationCurrentlyNotPlay();
        
        public bool IsPlayerDead() => _player.IsDead();

        private bool IsDirectionTypeAnimationCurrentlyNotPlay() =>
            _animatorHandler.GetInt(AnimatorParameter.HitDirectionType) == 0;
    }
}