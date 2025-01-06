namespace RehvidGames.Characters.Enemies
{
    using Animator;
    using Base;
    using Player;
    using Enums;
    using RehvidGames.Items.Weapons.Base;
    using Managers;
    using UnityEngine;

    public class EnemyCombat : MonoBehaviour
    {
        public AttackStateType CurrentAttackState { get; private set; } = AttackStateType.Ready;
        
        [SerializeField] private BaseEnemy _enemy;
        
        private PlayerController _player;
        
        [Tooltip("Fight radius in meters")]
        [SerializeField] private float _fightRadius = 2f;

        private BaseWeapon _weapon;
        private AnimatorHandler _animatorHandler;
        
        private void Start()
        {
            _player = GameManager.Instance.Player;

            if (!_enemy)
            {
                Debug.Log("Enemy not found");
                return;
            }
            
            _weapon = _enemy.WeaponHandler.CurrentWeapon;
            _animatorHandler = _enemy.AnimatorHandler;
        }

        #region Events
        public void OnEnemyEndAttack() => SetReadyAttackState();
        #endregion
        
        public void SetReadyAttackState() => CurrentAttackState = AttackStateType.Ready;
        
        public void Attack()
        {
            _animatorHandler.SetTrigger(AnimatorParameter.Attack);
            CurrentAttackState = AttackStateType.Attack;
        }
        
        public void ResetAttack()
        {
            CurrentAttackState = AttackStateType.WaitingForAnimation;
            if (!_enemy.IsDead())
            {
                ResetAttackAnimation();
            }
        }

        private void ResetAttackAnimation()
        {
            StartCoroutine(
                _animatorHandler.WaitForCurrentAnimationThenInvoke(() =>
                {
                    _animatorHandler.SetTrigger(AnimatorParameter.Attack);
                    _animatorHandler.SetInt(AnimatorParameter.HitDirectionType, 0);
                    CurrentAttackState = AttackStateType.Attack;
                })
            );
        }
        
        public bool CanAttack() => 
            Vector3.Distance(transform.position, _player.transform.position) <= _fightRadius 
            && !_player.IsDead()
            && CurrentAttackState == AttackStateType.Ready;

        public bool CanResetAttack() =>
            CurrentAttackState == AttackStateType.Attack && !IsDirectionTypeAnimationCurrentlyNotPlay();
        
        
        private bool IsDirectionTypeAnimationCurrentlyNotPlay() =>
            _animatorHandler.GetInt(AnimatorParameter.HitDirectionType) == 0;
    }
}