namespace RehvidGames.Characters.Enemies
{
    using Animator;
    using Base;
    using Enums;
    using Factories;
    using Managers;
    using Player;
    using UnityEngine;

    public class EnemyController: MonoBehaviour
    {
        
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

        public BaseEnemy Enemy => _enemy;
        
        public EnemyStateHandler StateHandler => _stateHandler;
        
        private BaseEnemy _enemy;
        private PlayerController _player;

        private bool _hasProcessedDeathStateExit;
        
        private void Start()
        {
            FindComponents();
            
            _player = GameManager.Instance.Player;
            _stateHandler?.InitState(_stateHandler.GetState(EnemyStateType.Patrol, this));
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
            if (GameManager.Instance.IsPaused || _hasProcessedDeathStateExit) return;

            HandleProcessingDeathStateExit();
            HandleStateManagement();
            UpdateAnimatorAgentSpeed();
        }

        private void HandleProcessingDeathStateExit()
        {
            if (!_enemy.IsDead()) return;
            
            _stateHandler.ExitCurrentState();
            _hasProcessedDeathStateExit = true;
        }
        
        
        private void HandleStateManagement()
        {
            _stateHandler.ExecuteCurrentState();
            _stateHandler.TransitionToNextState();
        }
        
        private void UpdateAnimatorAgentSpeed() 
            => _enemy.AnimatorHandler.SetFloat(AnimatorParameter.XSpeed, _movement.GetVelocityMagnitudeAgent());

        public bool IsPlayerDead() => _player.IsDead();

        public void OnPlayerDeath(Component sender, object value)
        {
            _movement.StopMovement();
        }
    }
}