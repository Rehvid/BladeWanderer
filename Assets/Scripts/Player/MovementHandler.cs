namespace RehvidGames.Player
{
    using System.Collections;
    using Animator;
    using Audio;
    using Enums;
    using UnityEngine;
    using UnityEngine.InputSystem;
    using VFXManager = Managers.VFXManager;

    public class MovementHandler: MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] private Player _player;
        [SerializeField] private CharacterController _characterController;

        [Header("Speed parameters")] 
        [Tooltip("Walking speed of the character.")] 
        [SerializeField] private float _walkSpeed = 5f;

        [Tooltip("Running speed of the character.")] 
        [SerializeField] private float _runSpeed = 10f;
        [Tooltip("Rate at which the character decelerates to a full stop when movement input is released. Higher values cause quicker stops, while lower values result in a slower, more gradual deceleration.")]
        [SerializeField] private float _decelerationRate = 5f;
        
        [Header("Camera parameters")]
        [Tooltip("Camera transform used to calculate direction for character movement.")]
        [SerializeField] private Transform _cameraTransform;
        [Tooltip("Time taken to smoothly rotate the character towards the movement direction.")] 
        [SerializeField] private float  _rotationSmoothingTime = 0.1f;
        [SerializeField] private bool _isLockingCamera;
        
        [Header("Animator parameters")]
        [Tooltip("Constant Y value used for vertical movement in the animator.")]
        [SerializeField] private float _animatorVerticalOffset = -0.5f;
        [Tooltip("Smoothing factor for speed transitions in the animator. Lower values make animations more responsive to speed changes, while higher values create smoother, slower adjustments.")]
        [SerializeField] private float _animatorSpeedSmoothingFactor = 0.1f;

        private bool _stopRunning;
        private bool _isRunHolding;
        private bool _isStopped;
        private bool _canInvokeVfxEffects;
        private bool _isPlayerFootstepsSoundPlaying;
        
        private float _fullRunStaminaCost => _player.StaminaCosts.Run * Time.deltaTime;
        private float _rotationVelocity;
        private float _currentSpeed;
        private float _previousSpeed;
        
        private Vector3 _movementInput;
        private Coroutine _speedCoroutine;
        private AnimatorHandler _animatorHandler;
        private AudioManager _audioManager;
        
        private void Start()
        {
            _animatorHandler = _player.AnimatorHandler;
            if (_isLockingCamera)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            _audioManager = AudioManager.Instance;
            _audioManager.PlayRandomClip(SoundType.PlayerJump);
        }

        private void Update()
        {
            HandlePlayerRunning();
            if (!_player.ActionManager.IsUnoccupied())
            {
                _animatorHandler.SetFloat(AnimatorParameter.XSpeed, 0);
                _isPlayerFootstepsSoundPlaying = false;
                return; 
            }
            HandleMovement();

            if (!_isStopped)
            {
                UpdateAnimatorParameters();
            }
            
            PlayMovementStopEffects();
            PlayMovementSound();
        }
        
        private void HandlePlayerRunning()
        {
            if (!HasEnoughStaminaToRun())
            {
                StopRunning();
                return;
            }

            ContinueRunning();
        }
        
        private bool HasEnoughStaminaToRun() => _player.HasEnoughStamina(_fullRunStaminaCost);

        private void StopRunning()
        {
            if (_currentSpeed > _walkSpeed)
            { 
                StartSpeedCoroutine(_walkSpeed);
            }
            _stopRunning = true;
                
            if (_player.IsRegenerationStarted())
            {
                _player.StopRegenerationStamina();
                return;
            }
            
            _player.RegenerationStamina();  
        }
        
        private void StartSpeedCoroutine(float targetSpeed)
        {
            if (_speedCoroutine != null)
            {
                StopCoroutine(_speedCoroutine);
            }
            _speedCoroutine = StartCoroutine(SmoothSpeedTransition(targetSpeed, InputSystem.settings.defaultHoldTime));
        }
        private IEnumerator SmoothSpeedTransition(float targetSpeed, float transitionTime)
        {
            float initialSpeed = _currentSpeed;
            float elapsedTime = 0f;
        
            while (elapsedTime < transitionTime)
            {
                elapsedTime += Time.deltaTime;
                float time = elapsedTime / transitionTime;
                _currentSpeed = Mathf.Lerp(initialSpeed, targetSpeed, time);
                yield return null;
            }
        
            _currentSpeed = targetSpeed;
        }
        
        private void ContinueRunning()
        {
            _stopRunning = false;
            if (_isRunHolding)
            {
                _player.UseStamina(_fullRunStaminaCost);
                return;
            }
          
            _player.RegenerationStamina();
        }

        private void HandleMovement()
        {
            if (!CanMove())
            {
                DecelerateToStop();
                return;
            }
            _canInvokeVfxEffects = true;
            ApplyRotation();
            MoveCharacter();
        }
        
        private bool CanMove() => _movementInput.magnitude >= 0.1f;
        
        private void DecelerateToStop()
        {
            _characterController.Move(new Vector3(0, _animatorVerticalOffset, 0));
            _currentSpeed = Mathf.Lerp(_currentSpeed, 0f, Time.deltaTime * _decelerationRate);
        }
        
        private void ApplyRotation()
        {
            var targetRotation = CalculateSmoothRotation();
            RotateCharacterTo(targetRotation);
        }
        
        private float CalculateSmoothRotation()
        {
            return Mathf.SmoothDampAngle(
                _characterController.transform.eulerAngles.y, 
                CalculateTargetRotationAngle(), 
                ref _rotationVelocity,
                _rotationSmoothingTime
            );
        }
        
        private float CalculateTargetRotationAngle()
        {
            var angleXZ = Mathf.Atan2(_movementInput.x, _movementInput.z) * Mathf.Rad2Deg;
            return angleXZ + _cameraTransform.eulerAngles.y; 
        }
        
        
        private void RotateCharacterTo(float targetRotation)
        {
            _characterController.transform.rotation = Quaternion.Euler(0f, targetRotation, 0f);
        }
        
        private void MoveCharacter()
        {
            var normalizedMoveDirection = NormalizeMoveDirection(CalculateMoveDirection());
            _characterController.Move(normalizedMoveDirection);
        }
        
        
        private Vector3 CalculateMoveDirection()
        {
            return Quaternion.Euler(0f, CalculateTargetRotationAngle(), 0f) * Vector3.forward; 
        }
        
        private Vector3 NormalizeMoveDirection(Vector3 moveDirection)
        {
            return moveDirection.normalized * (_currentSpeed * Time.deltaTime);
        }
        
        private void UpdateAnimatorParameters()
        {
            float currentSpeed = new Vector3(_characterController.velocity.x, 0, _characterController.velocity.z).magnitude;
            float interpolatedSpeed = Mathf.Lerp(_previousSpeed, currentSpeed,  _animatorSpeedSmoothingFactor);
            
            _animatorHandler.SetFloat(AnimatorParameter.XSpeed, interpolatedSpeed);
            _animatorHandler.SetFloat(AnimatorParameter.YSpeed, _animatorVerticalOffset);
            _previousSpeed = interpolatedSpeed;
            
            if (!(interpolatedSpeed <= 0.01)) return;
            _isStopped = true;
            _currentSpeed = 0;
        }

        private void PlayMovementStopEffects()
        {
            if (CanMove() || !_canInvokeVfxEffects) return;
            
            VFXManager.Instance.PlayParticleEffect(_player.CharacterEffects.DustVFX, transform.position);
            _canInvokeVfxEffects = false;
        }

        private void PlayMovementSound()
        {
            if (!CanMove())
            {
                if (_isPlayerFootstepsSoundPlaying)
                {
                    StopMovementSound();
                }
                return;
            }
            
            if (_isPlayerFootstepsSoundPlaying) return;
            AudioManager.Instance.PlayRandomClip(SoundType.PlayerFootsteps);
            _isPlayerFootstepsSoundPlaying = true;
        }

        private void StopMovementSound()
        {
            AudioManager.Instance.StopCurrentSoundType(SoundType.PlayerFootsteps);
            _isPlayerFootstepsSoundPlaying = false;
        }
        
        #region Input events
        public void OnMove(InputAction.CallbackContext context)
        {
            _isStopped = false;
            var inputMovement = context.ReadValue<Vector2>();
            _movementInput = new Vector3(inputMovement.x, 0f, inputMovement.y).normalized;
            if (!_isRunHolding || _currentSpeed < 1f)
            {
                _currentSpeed = _walkSpeed; 
            }
        }
        
        public void OnRun(InputAction.CallbackContext context)
        {
            if (_stopRunning) return;
            if (_player.IsRegenerationStarted())
            {
                _player.StopRegenerationStamina();
            }
            _isRunHolding = !context.canceled;
            StartSpeedCoroutine(context.canceled ? _walkSpeed : _runSpeed);
        }
        #endregion
    }
}