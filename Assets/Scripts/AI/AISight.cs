namespace RehvidGames.AI
{
    using Player;
    using UnityEngine;
    using UnityEngine.Events;

    public class AISight: MonoBehaviour
    {
        [SerializeField] private Player _player;
        
        [Header("Sight settings")] 
        [Tooltip("Detection radius in meters")]
        [SerializeField] private float _detectionRadius = 30f;
        
        [Tooltip("Field of view angle in degrees")]
        [SerializeField] private float _fieldOfViewAngle = 45f;

        [Tooltip("Mask for obstacles that block player visibility. If the player is behind one, they won't be detected. ")]
        [SerializeField] private LayerMask _obstacleMask;
        
        [Header("Debug")]
        [SerializeField] private bool _sightDebug;
        
        [Header("Events")] 
        [SerializeField] private UnityEvent<Vector3> _playerSightLostTriggered;
        [SerializeField] private UnityEvent _playerSightDetectedTriggered;
        
        private bool _wasPlayerDetectedLastFrame;
        
        private void Update()
        {
            if (!CanDetectPlayer()) return;
            DetectPlayer();
        }

        private void DetectPlayer()
        {
            var isPlayerCurrentlyVisible = IsPlayerVisible();
            
            if (isPlayerCurrentlyVisible != _wasPlayerDetectedLastFrame)
            {
                if (isPlayerCurrentlyVisible)
                {
                    TriggerPlayerDetected();
                }
                else
                {
                    TriggerPlayerLost();
                }
            }

            _wasPlayerDetectedLastFrame = isPlayerCurrentlyVisible;
        }
        
        private bool CanDetectPlayer() => _player && !_player.IsDead();

        private bool IsPlayerVisible()
        {
            var playerDirectionNormalized = CalculatePlayerDirectionNormalized();
            return IsPlayerInFieldOfViewAngle(playerDirectionNormalized) && 
                   IsPlayerSeen(playerDirectionNormalized, CalculateDistanceToPlayer());
        }
        private Vector3 CalculatePlayerDirectionNormalized()
        {
            return (_player.transform.position - transform.position).normalized;
        }
        
        private bool IsPlayerInFieldOfViewAngle(Vector3 playerDirectionNormalized)
        {
            return Vector3.Angle(transform.forward, playerDirectionNormalized) < _fieldOfViewAngle / 2;
        }
        
        private float CalculateDistanceToPlayer()
        {
            return Vector3.Distance(transform.position, _player.transform.position);
        }
        
        private bool IsPlayerSeen(Vector3 playerDirectionNormalized, float currentDistanceToPlayer)
        {
            return currentDistanceToPlayer <= _detectionRadius &&
                   IsNotBlockedByObstacle(playerDirectionNormalized, currentDistanceToPlayer);
        }
        
        private bool IsNotBlockedByObstacle(Vector3 playerDirectionNormalized, float currentDistanceToPlayer)
        {
            return !Physics.Raycast(transform.position, playerDirectionNormalized, currentDistanceToPlayer, _obstacleMask);
        }
        
        private void TriggerPlayerDetected()
        {
            _playerSightDetectedTriggered.Invoke();
        }

        private void TriggerPlayerLost()
        {
            _playerSightLostTriggered.Invoke(_player.transform.position);
        }
        
        private void OnDrawGizmos()
        {
            if (!_sightDebug) return;
            
            DrawFieldOfViewGizmos();
            DrawDetectionRadiusGizmo();
        }
        
        private void DrawFieldOfViewGizmos()
        {
            Gizmos.color = Color.yellow;
            Vector3 forward = transform.forward * _detectionRadius;
            Gizmos.DrawRay(transform.position, forward);

            Vector3 leftBoundary = Quaternion.Euler(0, -_fieldOfViewAngle / 2, 0) * transform.forward * _detectionRadius;
            Vector3 rightBoundary = Quaternion.Euler(0, _fieldOfViewAngle / 2, 0) * transform.forward * _detectionRadius;
            Gizmos.DrawRay(transform.position, leftBoundary);
            Gizmos.DrawRay(transform.position, rightBoundary);
        }

        private void DrawDetectionRadiusGizmo()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _detectionRadius);
        }
    }
}