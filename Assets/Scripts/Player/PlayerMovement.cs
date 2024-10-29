namespace RehvidGames.Player
{
    using System.Collections;
    using Animator;
    using Enums;
    using Managers;
    using UnityEngine;
    using UnityEngine.InputSystem;
    
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Configuration")] 
        [SerializeField] private AnimatorController _animator;
        [SerializeField] private Player _player;
        [SerializeField] private CharacterController _characterController;
        
        [Header("Movement Parameters")] 
        [SerializeField] private float _walkSpeed = 5.0f;
        [SerializeField] private float _runSpeed = 10.0f;
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private float _turnSmoothTime = 0.1f;
        private float _currentSmoothVelocity;
        private Vector3 _movementDirection;
    
        [Header("Jump Parameters")] 
        [SerializeField] private float _jumpPower = 3.0f;
        [SerializeField] private bool _isLockingCamera;
        [SerializeField] private int _staminaCost = 20;
        
        private bool _isJumpTriggered;
        private float _currentVerticalSpeed;
        private float _speed;
        private Coroutine _speedCoroutine;

        private bool IsParticleDustPlayedForMovement = false;
        
        private void Start()
        {
            _speed = _walkSpeed;
            Init();
        }

        private void Init()
        {
            if (_isLockingCamera)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }

            TryGetComponent(out _animator);
        }
        
    
        private void Update()
        {
            ApplyGravityAndGroundCheck();
            UpdateJump();
            UpdateMovement();
            UpdateAnimatorSpeed();
        }
    
        private void ApplyGravityAndGroundCheck()
        {
            _currentVerticalSpeed += Physics.gravity.y * Time.deltaTime;
            if (IsGrounded())
            {
                _currentVerticalSpeed = -0.5f; 
            }
        }
    
        private void UpdateJump()
        {
            if (!CanJump()) return;
            
            _player.UseStamina(_staminaCost);
            _player.SetAction(PlayerActionType.Jumping);
            _currentVerticalSpeed = _jumpPower;
            
            _animator.SetTrigger(AnimatorParameter.Jump);
        }
    
        private bool CanJump()
        {
            return (IsGrounded() && _isJumpTriggered)
                   && _player.ActionManager.IsUnoccupied()
                   && _player.HasEnoughStamina(_staminaCost);
            ;
        }
        
        private void UpdateMovement()
        {
            if (!CanMove())
            {
                ApplyGravity();
                return;
            }
            
            HandleRotation();
            MoveCharacter();
        }
        
        private void ApplyGravity()
        {
            var moveDirection = new Vector3(0f, _currentVerticalSpeed, 0f);
            if (_player.ActionManager.IsJumping())
            {
                moveDirection.x = _characterController.velocity.x;
                moveDirection.z = _characterController.velocity.z;
            }
            
            _characterController.Move(moveDirection * Time.deltaTime);
        }

        private void HandleRotation()
        {
            var smoothDampAngle = SmoothRotateTowardsTarget(CalculateTargetAngle());
            RotateCharacter(smoothDampAngle);
        }
        
        private float CalculateTargetAngle()
        {
            var angleXZ = Mathf.Atan2(_movementDirection.x, _movementDirection.z) * Mathf.Rad2Deg;
            return angleXZ + _cameraTransform.eulerAngles.y;
        }
        
        private float SmoothRotateTowardsTarget(float targetAngle)
        {
            return Mathf.SmoothDampAngle(
                transform.eulerAngles.y, 
                targetAngle, 
                ref _currentSmoothVelocity,
                _turnSmoothTime
            );
        }
        
        private void RotateCharacter(float smoothDampAngle)
        {
            transform.rotation = Quaternion.Euler(0f, smoothDampAngle, 0f);
        }
        
        private void MoveCharacter()
        {
            var normalizedMoveDirection = NormalizeMoveDirection(CalculateMoveDirection());
            _characterController.Move(normalizedMoveDirection);
        }
        
        private Vector3 CalculateMoveDirection()
        {
            var moveDirection = Quaternion.Euler(0f, CalculateTargetAngle(), 0f) * Vector3.forward; 
            moveDirection.y = _currentVerticalSpeed;
            return moveDirection;
        }

        private Vector3 NormalizeMoveDirection(Vector3 moveDirection)
        {
            return moveDirection.normalized * _speed * Time.deltaTime;
        }
        
        private bool IsMoving()
        {
            return _movementDirection.magnitude >= 0.1f;
        }

        private bool CanMove()
        {
            return IsMoving() && _player.ActionManager.IsUnoccupied();
        }
        
        private void UpdateAnimatorSpeed()
        {
            var velocity = _characterController.velocity;
            var currentHorizontalSpeed = new Vector3(velocity.x, 0, velocity.z);

            if (!IsMoving() && IsParticleDustPlayedForMovement == false)
            {
                PlayDustParticleEffect();
                IsParticleDustPlayedForMovement = true;
            }

            if (IsMoving())
            {
                IsParticleDustPlayedForMovement = false;
            }
            
            _animator.SetFloat(AnimatorParameter.XSpeed, currentHorizontalSpeed.magnitude);
            _animator.SetFloat(AnimatorParameter.YSpeed, _currentVerticalSpeed);
        }
        
        private bool IsGrounded()
        {
            return _characterController.isGrounded;
        }
        
        public void OnMove(InputAction.CallbackContext context)
        {
            var inputMovement = context.ReadValue<Vector2>();
            _movementDirection = new Vector3(inputMovement.x, 0f, inputMovement.y).normalized;
        }

        public void OnRun(InputAction.CallbackContext context)
        {
            float targetSpeed = _walkSpeed;
            if (!context.canceled)
            {
                targetSpeed = _runSpeed;
            }

            if (_speedCoroutine != null)
            {
                StopCoroutine(_speedCoroutine);
            }
            
            _speedCoroutine = StartCoroutine(SmoothSpeedTransition(targetSpeed, InputSystem.settings.defaultHoldTime));
        }

        private IEnumerator SmoothSpeedTransition(float targetSpeed, float speedTransitionTime)
        {
            float initialSpeed = _speed;
            float elapsedTime = 0f;

            while (elapsedTime < speedTransitionTime)
            {
                elapsedTime += Time.deltaTime;
                float time = elapsedTime / speedTransitionTime;
                _speed = Mathf.Lerp(initialSpeed, targetSpeed, time);
                yield return null;
            }

            _speed = targetSpeed;
        }
        
        
        public void OnJump(InputAction.CallbackContext context)
        {
            _isJumpTriggered = context.performed;
        }
        
        public void OnFallingLand()
        {
            _player.SetAction(PlayerActionType.Unoccupied);
            PlayDustParticleEffect();
        }

        public void OnFallingIdleEnd()
        {
            _player.SetAction(PlayerActionType.IdleFalling);
        }

        private void PlayDustParticleEffect()
        {
            VFXManager.Instance.PlayParticleEffect(_player.CharacterEffects.DustVFX, _player.transform.position);
        }
    }
}
