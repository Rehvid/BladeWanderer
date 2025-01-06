namespace RehvidGames.Characters.Enemies
{
    using System.Collections.Generic;
    using UnityEngine;

    public class EnemyMovementPatrol: MonoBehaviour
    {
        public bool IsImmediatePatrol { get; set; }
        public bool IsWaitingForNextPatrolTarget { get; set; }
        
        [SerializeField] private Transform[] _patrolTargets;
        [SerializeField] private float _waitSecondsForNextPointTarget = 15f;
        
        public float WaitSecondsForNextPointTarget => _waitSecondsForNextPointTarget;
        
        private readonly List<Transform> _availablePatrolTargets = new();
        private Transform _currentPatrolTarget;
        private Transform _previousPatrolTarget;
        
        public Transform TakePatrolTarget()
        {
            _availablePatrolTargets.Clear();
            UpdateAvailablePatrolTargets();

            if (_availablePatrolTargets.Count <= 0) return null;
            SelectNextPatrolTarget();
            
            return _currentPatrolTarget;
        }

        private void UpdateAvailablePatrolTargets()
        {
            foreach (var target in _patrolTargets)
            {
                if (target != _currentPatrolTarget && target != _previousPatrolTarget)
                {
                    _availablePatrolTargets.Add(target);
                }
            }
        }

        private void SelectNextPatrolTarget()
        {
            _previousPatrolTarget = _currentPatrolTarget;
            
            var patrolTargetIndex = Random.Range(0, _availablePatrolTargets.Count);
            
            _currentPatrolTarget = _availablePatrolTargets[patrolTargetIndex];
        }
        
        public bool CanTakePatrolTarget() => _patrolTargets.Length > 0 && !IsWaitingForNextPatrolTarget;
    }
}