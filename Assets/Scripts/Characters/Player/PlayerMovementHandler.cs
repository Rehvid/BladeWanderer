namespace RehvidGames.Characters.Player
{
    using System.Collections;
    using Audio;
    using Managers;
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class PlayerMovementHandler: MonoBehaviour
    {
        private const float MinMovementThreshold = 0.1f;
        
        [Header("Configuration")]
        [SerializeField] private CharacterController _characterController;
        [Tooltip("Rate at which the character decelerates to a full stop when movement input is released. Higher values cause quicker stops, while lower values result in a slower, more gradual deceleration.")]
        [SerializeField] private float _decelerationRate = 5f;
        
        [Header("Camera parameters")]
        [Tooltip("Camera transform used to calculate direction for character movement.")]
        [SerializeField] private Transform _cameraTransform;
        [Tooltip("Time taken to smoothly rotate the character towards the movement direction.")] 
        [SerializeField] private float  _rotationSmoothingTime = 0.1f;
        
        public CharacterController CharacterController => _characterController;
        
        public bool IsRunHolding { get; set; }
        
        public bool ShouldStopRunning { get; private set; }
        
        public Vector3 MovementInput { get; set; }
        
        public float CurrentSpeed { get; set; }
        
        private PlayerController _player;
        private AudioManager _audioManager;
        private Coroutine _speedCoroutine;
        
        private float _rotationVelocity;
        
        private void Start()
        {
            FindComponents();
        }

        private void FindComponents()
        {
            _player = GameManager.Instance.Player;
            if (_player == null)
            {
                Debug.LogWarning("Player not found in");
            }
            _audioManager = AudioManager.Instance;
            if (_audioManager == null)
            {
                Debug.LogWarning("Audio manager not found in");
            }
        }
        
        public bool IsPlayerInAction() => !_player.ActionHandler.IsUnoccupied();

        private float RunStaminaCost() => _player.StaminaCostsData.BaseRun * Time.deltaTime;
        
        public void HandleRunning(float walkSpeed)
        {
            if (!_player.HasEnoughStamina(RunStaminaCost()))
            {
                StopRunning(walkSpeed);
                return;
            }

            ContinueRunning();
        }
        
        private void StopRunning(float walkSpeed)
        {
            if (CurrentSpeed > walkSpeed)
            { 
                StartSpeedCoroutine(walkSpeed);
            }
            
            ShouldStopRunning = true;
            
            HandleStamina();
        }
        
        public void StartSpeedCoroutine(float targetSpeed)
        {
            if (_speedCoroutine != null)
            {
                StopCoroutine(_speedCoroutine);
            }
            _speedCoroutine = StartCoroutine(SmoothSpeedTransition(targetSpeed, InputSystem.settings.defaultHoldTime));
        }
        private IEnumerator SmoothSpeedTransition(float targetSpeed, float transitionTime)
        {
            float initialSpeed = CurrentSpeed;
            var elapsedTime = 0f;
        
            while (elapsedTime < transitionTime)
            {
                elapsedTime += Time.deltaTime;
                float time = elapsedTime / transitionTime;
                CurrentSpeed = Mathf.Lerp(initialSpeed, targetSpeed, time);
                yield return null;
            }
        
            CurrentSpeed = targetSpeed;
        }

        private void HandleStamina()
        {
            if (_player.IsRegenerationStarted())
            {
                _player.StopRegenerationStamina();
                return;
            }
            
            _player.RegenerationStamina(); 
        }
        
        private void ContinueRunning()
        {
            ShouldStopRunning = false;
            if (IsRunHolding)
            {
                _player.UseStamina(RunStaminaCost());
                return;
            }
          
            _player.RegenerationStamina();
        }
        
        public void HandleMovement()
        {
            ApplyRotation();
            MoveCharacter();
        }
        
        public bool CanMove() => MovementInput.magnitude >= MinMovementThreshold;
        
        public void DecelerateToStop(float animatorVerticalOffset)
        {
            _characterController.Move(new Vector3(0, animatorVerticalOffset, 0));
            CurrentSpeed = Mathf.Lerp(CurrentSpeed, 0f, Time.deltaTime * _decelerationRate);
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
            var angleXZ = Mathf.Atan2(MovementInput.x, MovementInput.z) * Mathf.Rad2Deg;
            return angleXZ + _cameraTransform.eulerAngles.y; 
        }
        
        private void RotateCharacterTo(float targetRotation) 
            => _characterController.transform.rotation = Quaternion.Euler(0f, targetRotation, 0f);
        
        private void MoveCharacter()
        {
            var normalizedMoveDirection = NormalizeMoveDirection(CalculateMoveDirection());
            _characterController.Move(normalizedMoveDirection);
        }
        
        private Vector3 CalculateMoveDirection() => Quaternion.Euler(0f, CalculateTargetRotationAngle(), 0f) * Vector3.forward; 
        
        private Vector3 NormalizeMoveDirection(Vector3 moveDirection) => moveDirection.normalized * (CurrentSpeed * Time.deltaTime);
    }
}