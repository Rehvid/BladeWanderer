namespace RehvidGames.Characters.Enemies
{
    using AI;
    using Animator;
    using Base;
    using Enums;
    using Factories;
    using Managers;
    using Player;
    using UnityEngine;

    public class EnemyController: MonoBehaviour
    {
        private BaseEnemy _enemy;
        private PlayerController _player;
        
        [Header("State handler")]
        [SerializeField] private EnemyStateHandler _stateHandler;

        [Header("Components")] 
        [SerializeField] private EnemyMovement _movement;
        [SerializeField] private EnemySight _sight;
        [SerializeField] private EnemyCombat _combat;
        
        public PlayerController Player => _player;
        
        public EnemyMovement Movement => _movement;
        
        public EnemySight Sight => _sight;
        
        public EnemyCombat Combat => _combat;
        
        private void Start()
        {
            FindComponents();
            _player = GameManager.Instance.Player;
            _stateHandler?.InitState(EnemyStateFactory.GetState(EnemyStateType.Patrol, this));
        }

        private void FindComponents()
        {
            if (!TryGetComponent(out BaseEnemy enemy))
            {
                Debug.Log("Enemy not found");
                gameObject.SetActive(false);
                return;
            }

            _enemy = enemy;
        }

        private void Update()
        {
            if (!CanUpdate()) return;
            
            HandleStateManagement();
            UpdateAnimatorAgentSpeed();
        }
        
        private bool CanUpdate() => !_enemy.IsDead() || !GameManager.Instance.IsPaused;
        
        private void HandleStateManagement()
        {
            _stateHandler.ExecuteCurrentState();
            _stateHandler.TransitionToNextState();
        }
        
        private void UpdateAnimatorAgentSpeed() 
            => _enemy.AnimatorHandler.SetFloat(AnimatorParameter.XSpeed, _movement.GetVelocityMagnitudeAgent());

        public bool IsPlayerDead() => _player.IsDead();
    }
}