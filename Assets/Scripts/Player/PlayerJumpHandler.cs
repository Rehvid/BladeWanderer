namespace RehvidGames.Player
{
    using Animator;
    using Enums;
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class PlayerJumpHandler : MonoBehaviour
    {
        [SerializeField] private int _staminaCost = 20;
        [SerializeField] private float _jumpPower = 3.0f;
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private Player _player;
        [SerializeField] private float _forwardJumpDistance = 5.0f;    
        
        private AnimatorController _animatorController;
        private bool _isJumpTriggered;
        private float _currentVerticalSpeed;
        private Vector3 _jumpDirection;
        
        private const float GravityScale = -2f;

        public void OnJump(InputAction.CallbackContext context)
        {
            _isJumpTriggered = context.performed;
        }

        private void Start()
        {
            if (_player)
            {
                _animatorController = _player.AnimatorController;
            }
        }

        private void Update()
        {
            HandleJumpInput();
            ApplyJumpMovement();
            UpdateAnimatorParameters();
        }

        private void HandleJumpInput()
        {
            if (_isJumpTriggered && CanJump())
            {
                StartJump();
            }
        }
        
        private void StartJump()
        {
            _animatorController.SetTrigger(AnimatorParameter.Jump);
            _player.UseStamina(_staminaCost);
            _player.SetAction(PlayerActionType.Jumping);
            _currentVerticalSpeed = CalculateJumpSpeed(); 
            _jumpDirection = DetermineJumpDirection();
        }

        private float CalculateJumpSpeed()
        {
            return Mathf.Sqrt(_jumpPower * GravityScale * Physics.gravity.y);
        }
        
        private Vector3 DetermineJumpDirection()
        {
            if (_characterController.velocity.magnitude > 0.1f)
            {
                _animatorController.SetBool(AnimatorParameter.JumpRun, true);
                return new Vector3(_characterController.velocity.x, 0, _characterController.velocity.z).normalized * _forwardJumpDistance;
            }
            return Vector3.zero; 
        }

        private void ApplyJumpMovement()
        {
            if (!_player.ActionManager.IsJumping()) return;
          
            _currentVerticalSpeed += Physics.gravity.y * Time.deltaTime;
            Vector3 movement = _jumpDirection + Vector3.up * _currentVerticalSpeed;
            _characterController.Move(movement * Time.deltaTime);
            
            if (_characterController.isGrounded)
            {
                EndJump();
            }
        }

        private void EndJump()
        {
            _player.SetAction(PlayerActionType.Unoccupied);
            _currentVerticalSpeed = 0;
            _animatorController.SetBool(AnimatorParameter.JumpRun, false);
        }

        private void UpdateAnimatorParameters()
        {
            _animatorController.SetFloat(AnimatorParameter.YSpeed, _currentVerticalSpeed);
        }

        private bool CanJump()
        {
            return _characterController.isGrounded && _player.ActionManager.IsUnoccupied() &&
                   _player.Attributes.HasEnoughStaminaToMakeAction(_staminaCost);
        }
    }
}