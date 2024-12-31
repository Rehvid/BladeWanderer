namespace RehvidGames.Characters.Enemies
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.AI;

    public class EnemyMovement : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private float _defaultSpeed = 3.5f;
        [SerializeField] private float _chaseSpeed = 5f;

        [Header("Components")]
        [SerializeField] private EnemyMovementPatrol _movementPatrol;
        
        public EnemyMovementPatrol MovementPatrol => _movementPatrol;
        
        public void Patrol()
        {
            if (CanPatrol())
            {
                HandlePatrol();
            }
        }

        private void HandlePatrol()
        {
            Transform target = _movementPatrol.TakePatrolTarget();
            if (_movementPatrol.IsImmediatePatrol)
            {
                SetDestination(target.position);
            }
            else
            {
                StartCoroutine(MoveToDestinationTarget(target.position));
            }
        }

        private bool CanPatrol() => _movementPatrol.CanTakePatrolTarget() && IsTargetDestinationReach();
        
        private IEnumerator MoveToDestinationTarget(Vector3 target)
        {
            _movementPatrol.IsWaitingForNextPatrolTarget = true;
            yield return new WaitForSeconds(_movementPatrol.WaitSecondsForNextPointTarget);
            
            SetDestination(target);
            
            _movementPatrol.IsWaitingForNextPatrolTarget = false;
        }
        
        public void StopMovement()
        {
            _agent.velocity = Vector3.zero;
            _agent.isStopped = true;
        }
        
        public void ResumeMovement() => _agent.isStopped = false;
        
        public void SetDestination(Vector3 target) => _agent.SetDestination(target);
        
        public void SetChaseSpeed() => _agent.speed = _chaseSpeed;
        
        public void SetDefaultSpeed() => _agent.speed = _defaultSpeed;
        
        public bool IsTargetDestinationReach() => !_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance;
        
        public float GetVelocityMagnitudeAgent() => _agent.velocity.magnitude;
    }
}