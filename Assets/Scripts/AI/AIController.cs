namespace RehvidGames.AI
{
    using Animator;
    using Enums;
    using Managers;
    using Player;
    using UnityEngine;
    using Vector3 = UnityEngine.Vector3;

    public class AIController : MonoBehaviour
    {
        [Header("AI Components")]
        [SerializeField] private AIStateManagement _aiStateManagement;
        [SerializeField] private AIMovement _aiMovement;
        [SerializeField] private AIFight _aiFight;
        [SerializeField] private AIDetecting _aiDetecting;
        
        [Header("Settings")]
        [SerializeField] private AnimatorHandler _animatorHandler;
        public AnimatorHandler Animator => _animatorHandler;

        private bool _isDead;
        
        private void Start()
        {
            _aiStateManagement.InitState(this);
        }
        
        private void Update()
        {
            if (_isDead || GameManager.Instance.IsPaused) return;
            _aiStateManagement.ExecuteCurrentState();
            _aiStateManagement.TransitionToNextState();
            UpdateAnimatorAgentSpeed();
        }
        
        private void UpdateAnimatorAgentSpeed()
        {
            Animator.SetFloat(AnimatorParameter.XSpeed,_aiMovement.GetVelocityMagnitudeAgent());
        }

        #region Events

        public void OnDeathEnemy()
        {
            _isDead = true;
        }
        
        public void OnDeathPlayer()
        {
            _aiDetecting.SetIsPlayerDetected(false);
            _aiMovement.SetImmediatePatrol(true);
        }

        #endregion
      
        
        #region AI Movement
        public void StartPatrolling() => _aiMovement.Patrol();
        
        public void ResumeMovement() => _aiMovement?.ResumeMovement();
        
        public void StopMovement() => _aiMovement?.StopMovement();
        
        public void SetDestination(Vector3 target) => _aiMovement?.SetDestination(target);
        
        public void SetChaseSpeed() => _aiMovement?.SetChaseSpeed();
        
        public void SetDefaultSpeed() => _aiMovement?.SetDefaultSpeed();
        
        public void SetImmediatePatrol(bool value) => _aiMovement?.SetImmediatePatrol(value);
        
        public bool GetImmediatePatrol() => _aiMovement && _aiMovement.GetImmediatePatrol();
        
        public bool IsDestinationReach() => _aiMovement && _aiMovement.IsDestinationReach();
        
        #endregion AI Movement
        
        #region AI Fight
        
        public bool CanAttack() => _aiFight && _aiFight.CanAttack();
        
        public bool IsPlayerDead() => _aiFight && _aiFight.IsPlayerDead();
        
        public void Attack() => _aiFight?.Attack();

        public void ResetAttack() => _aiFight.ResetAttack();

        public AttackStateType CurrentAttackState => _aiFight.CurrentAttackState;

        public void SetReadyAttackState() => _aiFight?.SetReadyAttackState();

        public bool CanResetAttack() => _aiFight && _aiFight.CanResetAttack();
        
        #endregion
        
        #region AI Detecting
        public void SetDefaultLastKnownLocationPlayer() => _aiDetecting?.SetDefaultLastKnownLocationPlayer();
        
        public Player DetectedPlayer => _aiDetecting?.Player;
        
        public Vector3 LastKnownLocationPlayer => _aiDetecting.LastKnownLocationPlayer;
        
        public bool HasLastKnownLocationPlayer() => _aiDetecting && _aiDetecting.HasLastKnownLocationPlayer();
        
        public bool IsPlayerDetected() => _aiDetecting && _aiDetecting.IsPlayerDetected;
        
        #endregion AI Detecting
    }
}
