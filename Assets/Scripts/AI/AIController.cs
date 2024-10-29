namespace RehvidGames.AI
{
    using Animator;
    using Player;
    using Serializable;
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
        [SerializeField] private AnimatorController animatorController;
        
        private void Start()
        {
            _aiStateManagement.InitState(this);
        }
        
        private void Update()
        {
            _aiStateManagement.ExecuteCurrentState();
            _aiStateManagement.TransitionToNextState();
            UpdateAnimatorAgentSpeed();
        }
        
        private void UpdateAnimatorAgentSpeed()
        {
            PlayAnimation(
                AnimatorParameter.GetParameterName(AnimatorParameter.XSpeed), 
                AnimatorParameterType.Float, 
                _aiMovement.GetVelocityMagnitudeAgent()
            );
        }
        
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
        public void StartAttack() => _aiFight?.StartAttack();
        
        public void StopAttack() => _aiFight?.StopAttack();
        
        public bool CanAttack() => _aiFight && _aiFight.CanAttack();
        
        public bool IsPlayerDead() => _aiFight && _aiFight.IsPlayerDead();
        
        #endregion
        
        #region AI Detecting
        public void SetDefaultLastKnownLocationPlayer() => _aiDetecting?.SetDefaultLastKnownLocationPlayer();
        
        public Player DetectedPlayer => _aiDetecting?.Player;
        
        public Vector3 LastKnownLocationPlayer => _aiDetecting.LastKnownLocationPlayer;
        
        public bool HasLastKnownLocationPlayer() => _aiDetecting && _aiDetecting.HasLastKnownLocationPlayer();
        
        public bool IsPlayerDetected() => _aiDetecting && _aiDetecting.IsPlayerDetected;
        
        #endregion AI Detecting
        
        public void PlayAnimation(string animationName, AnimatorParameterType animatorParameterType, object value = null)
        {
            // animatorController.PlayAnimation(animationName, animatorParameterType, value);
        }
    }
}
