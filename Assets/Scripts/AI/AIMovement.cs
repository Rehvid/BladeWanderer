namespace RehvidGames.AI
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.AI;
    using Random = UnityEngine.Random;

    public class AIMovement : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private float _defaultSpeed = 3.5f;
        [SerializeField] private float _chaseSpeed = 5f;
        
        [Header("Patrol settings")]
        [SerializeField] private Transform[] _patrolTargets;
        [SerializeField] private float _waitSecondsForNextPointTarget = 15f;
        
        private readonly List<Transform> _availablePatrolTargets = new();
        private Transform _currentPatrolTarget;
        private Transform _previousPatrolTarget;
        private bool _isWaiting;
        private bool _isImmediatePatrol;
        
        public void Patrol()
        {
            if (!CanStartPatrolling()) return;
            
            TakePatrolTarget();
            if (_isImmediatePatrol)
            {
                SetDestination(_currentPatrolTarget.position);
                return;
            }
            StartCoroutine(MoveToTarget());
        }

        private bool CanStartPatrolling() => _patrolTargets.Length > 0 && IsDestinationReach() && !_isWaiting;
        
        private void TakePatrolTarget()
        {
            _availablePatrolTargets.Clear();
            
            foreach (var target in _patrolTargets)
            {
                if (target != _currentPatrolTarget && target != _previousPatrolTarget)
                {
                    _availablePatrolTargets.Add(target);
                }
            }
            
            if (_availablePatrolTargets.Count > 0)
            {
                _previousPatrolTarget = _currentPatrolTarget;
                
                var patrolTargetIndex = Random.Range(0, _availablePatrolTargets.Count);
                _currentPatrolTarget = _availablePatrolTargets[patrolTargetIndex];
            }
        }
        
        private IEnumerator MoveToTarget()
        {
            _isWaiting = true;
            yield return new WaitForSeconds(_waitSecondsForNextPointTarget);
            
            SetDestination(_currentPatrolTarget.position);
            _isWaiting = false;
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

        public void SetImmediatePatrol(bool value) => _isImmediatePatrol = value;

        public bool GetImmediatePatrol() => _isImmediatePatrol;
        
        public bool IsDestinationReach() => !_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance;
        
        public float GetVelocityMagnitudeAgent() => _agent.velocity.magnitude;
    }
}