namespace RehvidGames.Player
{
    using Animator;
    using Data.Serializable;
    using Enums;
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class PlayerMovement : MonoBehaviour
    {
      
        [Header("Configuration")] 
        [SerializeField] private AnimatorController animatorController;
        [SerializeField] private Player _player;
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private ParticleSystem _dust;
        
        private bool _isJumpTriggered;
        
        [Header("Movement Parameters")] 
        [SerializeField] private float _speed = 5.0f;
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private float _turnSmoothTime = 0.1f;
        private float _currentSmoothVelocity;
        private Vector3 _movementDirection;
    
        [Header("Jump Parameters")] 
        [SerializeField] private float _jumpPower = 3.0f;
        [SerializeField] private bool _isLockingCamera;
        [SerializeField] private int _staminaCost = 20;
        private float _currentVerticalSpeed;
        
        private void Start()
        {
            Init();
        }

        private void Init()
        {
            if (_isLockingCamera)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
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
            animatorController.PlayAnimation(
                AnimatorParameter.GetParameterName(AnimatorParameter.Jump), 
                AnimatorParameterType.Trigger
            );
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
            _dust.Play();
            
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
            animatorController.PlayAnimation(
                AnimatorParameter.GetParameterName(AnimatorParameter.XSpeed), 
                AnimatorParameterType.Float, 
                currentHorizontalSpeed.magnitude
            );
            animatorController.PlayAnimation(
                AnimatorParameter.GetParameterName(AnimatorParameter.YSpeed), 
                AnimatorParameterType.Float, 
                _currentVerticalSpeed
            );
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
   
        public void OnJump(InputAction.CallbackContext context)
        {
            _isJumpTriggered = context.performed;
        }
        
        public void OnFallingLand()
        {
            _player.SetAction(PlayerActionType.Unoccupied);
        }

        public void OnFallingIdleEnd()
        {
            _player.SetAction(PlayerActionType.IdleFalling);
        }
    }
}
