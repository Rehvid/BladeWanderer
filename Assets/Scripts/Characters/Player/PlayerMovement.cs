namespace RehvidGames.Characters.Player
{
    using Animator;
    using Audio;
    using Enums;
    using Managers;
    using UnityEngine;
    using UnityEngine.InputSystem;
    using VFXManager = VFX.VFXManager;
    
    public class PlayerMovement: MonoBehaviour
    {
        private const float MinimumRunningSpeed = 1f;
        
        [Header("Configuration")]
        [SerializeField] private PlayerMovementHandler _movementHandler;
        [SerializeField] private bool _isLockingCamera;
        
        [Header("Speed parameters")] 
        [Tooltip("Walking speed of the character.")] 
        [SerializeField] private float _walkSpeed = 5f;
        
        [Tooltip("Running speed of the character.")] 
        [SerializeField] private float _runSpeed = 10f;
        
        [Header("Animator parameters")]
        [Tooltip("Constant Y value used for vertical movement in the animator.")]
        [SerializeField] private float _animatorVerticalOffset = -0.5f;
        [Tooltip("Smoothing factor for speed transitions in the animator. Lower values make animations more responsive to speed changes, while higher values create smoother, slower adjustments.")]
        [SerializeField] private float _animatorSpeedSmoothingFactor = 0.1f;
        
        private bool _isStopped;
        private bool _canPlayDustGroundVFX;
        private bool _isFootstepsSoundPlaying;
        private float _previousSpeed;
         
        private PlayerController _player;
        private AnimatorHandler _animatorHandler;
        private AudioManager _audioManager;
        
        private void Start()
        {
            _player = GameManager.Instance.Player;
            if (_player == null)
            {
                Debug.LogWarning($"Player not found in");
            }
            
            _animatorHandler = _player.AnimatorHandler;
            if (_isLockingCamera)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            
            _audioManager = AudioManager.Instance;
        }

        private void Update()
        {
            if (GameManager.Instance.IsPaused) return;
            
            HandlePlayerMovement();
            if (!_isStopped)
            {
                UpdateAnimatorParameters();
            }
            
            PlayMovementStopEffects();
            PlayMovementSound();
        }

        private void HandlePlayerMovement()
        {
            _movementHandler.HandleRunning(_walkSpeed);
            
            if (_movementHandler.IsPlayerInAction())
            {
                StopPlayerMovementInAction();
                return;
            }

            if (!_movementHandler.CanMove())
            {
                _movementHandler.DecelerateToStop(_animatorVerticalOffset);
                return;
            }
            
            _canPlayDustGroundVFX = true;
            _movementHandler.HandleMovement();
        }

        private void StopPlayerMovementInAction()
        {
            _animatorHandler.SetFloat(AnimatorParameter.XSpeed, 0);
            if (_isFootstepsSoundPlaying)
            {
                _audioManager.StopCurrentSoundType(SoundType.PlayerFootsteps);
            }
            _isFootstepsSoundPlaying = false;
        }
      
        private void UpdateAnimatorParameters()
        {
            float interpolatedSpeed = CalculateInterpolatedSpeed();
            
            _animatorHandler.SetFloat(AnimatorParameter.XSpeed, interpolatedSpeed);
            _animatorHandler.SetFloat(AnimatorParameter.YSpeed, _animatorVerticalOffset);
            _previousSpeed = interpolatedSpeed;
            
            HandleMovementStop(interpolatedSpeed);
        }

        private float CalculateInterpolatedSpeed()
        {
            float currentSpeed = new Vector3(
                _movementHandler.CharacterController.velocity.x, 
                0, 
                _movementHandler.CharacterController.velocity.z
            ).magnitude;
            
            return Mathf.Lerp(_previousSpeed, currentSpeed,  _animatorSpeedSmoothingFactor);
        }

        private void HandleMovementStop(float interpolatedSpeed)
        {
            if (!(interpolatedSpeed <= Mathf.Epsilon)) return;
            
            _isStopped = true;
            _movementHandler.CurrentSpeed = 0;
        }

        private void PlayMovementStopEffects()
        {
            if (!ShouldPlayDustGroundVFX()) return;
            
            VFXManager.Instance.PlayParticleEffect(_player.CharacterEffectsData.DustGroundVFX, transform.position);
            _canPlayDustGroundVFX = false;
        }

        private bool ShouldPlayDustGroundVFX() => !_movementHandler.CanMove() && _canPlayDustGroundVFX;

        private void PlayMovementSound()
        {
            if (!_movementHandler.CanMove())
            {
                if (_isFootstepsSoundPlaying)
                {
                    StopMovementSound();
                }
                return;
            }
            
            if (IsFootstepsSoundPlaying())
            {
                _audioManager.PlayRandomClip(SoundType.PlayerFootsteps);
            }
            
            if (_isFootstepsSoundPlaying) return;
            
            _audioManager.PlayRandomClip(SoundType.PlayerFootsteps);
            _isFootstepsSoundPlaying = true;
        }
        
        private bool IsFootstepsSoundPlaying()
        {
            var currentContext = _audioManager.GetCurrentAudioPlayContext(SoundType.PlayerFootsteps);
            
            return _isFootstepsSoundPlaying && currentContext?.SoundType != SoundType.PlayerFootsteps;
        }

        private void StopMovementSound()
        {
            _audioManager.StopCurrentSoundType(SoundType.PlayerFootsteps);
            _isFootstepsSoundPlaying = false;
        }
        
        #region Input events
        public void OnMovePerformed(InputAction.CallbackContext context)
        {
            _isStopped = false;
            var inputMovement = context.ReadValue<Vector2>();
            
            _movementHandler.MovementInput = new Vector3(inputMovement.x, 0f, inputMovement.y).normalized;
            
            if (!_movementHandler.IsRunHolding || _movementHandler.CurrentSpeed < MinimumRunningSpeed)
            {
                _movementHandler.CurrentSpeed = _walkSpeed; 
            }
        }
        
        public void OnRunPerformed(InputAction.CallbackContext context)
        {
            if (_movementHandler.ShouldStopRunning) return;
            
            if (_player.IsRegenerationStarted())
            {
                _player.StopRegenerationStamina();
            }
            
            _movementHandler.IsRunHolding = !context.canceled;
            _movementHandler.StartSpeedCoroutine(context.canceled ? _walkSpeed : _runSpeed);
        }
        #endregion
    }
}