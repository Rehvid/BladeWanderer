namespace RehvidGames.Characters.Enemies
{
    using Player;
    using Managers;
    using UnityEngine;

    public class EnemySight : MonoBehaviour
    {
        public bool IsPlayerInSight { get; private set; }
        public Vector3 PlayerLastKnownLocation { get; private set; }
        
        private PlayerController _player;

        [Header("Sight settings")] 
        [Tooltip("Detection radius in meters")] [SerializeField] private float _detectionRadius = 30f;

        [Tooltip("Field of view angle in degrees")] [SerializeField] private float _fieldOfViewAngle = 45f;

        [Tooltip("Mask for obstacles that block player visibility. If the player is behind one, they won't be detected. ")]
        [SerializeField] private LayerMask _obstacleMask;

        [Header("Debug")] 
        [SerializeField] private bool _sightDebug;
        
        private bool _wasPlayerDetectedLastFrame;

        public void SetDefaultLastKnownLocationPlayer() => PlayerLastKnownLocation = Vector3.zero;
        
        private void Start()
        {
            _player = GameManager.Instance.Player;
        }

        private void Update()
        {
            if (!CanDetectPlayer()) return;

            DetectPlayer();
        }

        private bool CanDetectPlayer() => _player && !_player.IsDead();

        private void DetectPlayer()
        {
            var isPlayerCurrentlyVisible = IsPlayerVisible();

            if (isPlayerCurrentlyVisible != _wasPlayerDetectedLastFrame)
            {
                if (isPlayerCurrentlyVisible)
                {
                    IsPlayerInSight = true;
                }
                else
                {
                    IsPlayerInSight = false;
                    PlayerLastKnownLocation = _player.transform.position;
                }
            }

            _wasPlayerDetectedLastFrame = isPlayerCurrentlyVisible;
        }

        private bool IsPlayerVisible()
        {
            var playerDirectionNormalized = CalculatePlayerDirectionNormalized();

            return IsPlayerInFieldOfViewAngle(playerDirectionNormalized) &&
                   IsPlayerSeen(playerDirectionNormalized, CalculateDistanceToPlayer());
        }

        private Vector3 CalculatePlayerDirectionNormalized() =>
            (_player.transform.position - transform.position).normalized;

        private bool IsPlayerInFieldOfViewAngle(Vector3 playerDirectionNormalized)
            => Vector3.Angle(transform.forward, playerDirectionNormalized) < _fieldOfViewAngle / 2;

        private float CalculateDistanceToPlayer() => Vector3.Distance(transform.position, _player.transform.position);

        private bool IsPlayerSeen(Vector3 playerDirectionNormalized, float currentDistanceToPlayer)
        {
            return currentDistanceToPlayer <= _detectionRadius &&
                   IsNotBlockedByObstacle(playerDirectionNormalized, currentDistanceToPlayer);
        }

        private bool IsNotBlockedByObstacle(Vector3 playerDirectionNormalized, float currentDistanceToPlayer)
            => !Physics.Raycast(transform.position, playerDirectionNormalized, currentDistanceToPlayer, _obstacleMask);
        
        #region Debug
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

        #endregion
    }
}