namespace RehvidGames.Player
{
    using Animator;
    using Audio;
    using Enums;
    using Managers;
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class JumpHandler : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] private Player _player;
        [SerializeField] private CharacterController _characterController;
        
        [Header("Jump parameters")]
        [SerializeField] private float _jumpPower = 3.0f;
        [SerializeField] private float _forwardJumpDistance = 5.0f;  
        
        private AnimatorHandler _animatorHandler;
        private bool _isJumpTriggered;
        private float _currentVerticalSpeed;
        private Vector3 _jumpDirection;
        
        private const float GravityScale = -2f;

        private void Start()
        {
            if (_player)
            {
                _animatorHandler = _player.AnimatorHandler;
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
            _animatorHandler.SetTrigger(AnimatorParameter.Jump);
            AudioManager.PlayRandomAudioOneShot(SoundType.Jump);
            _player.UseStamina(_player.StaminaCosts.Jump);
            _player.RegenerationStamina();
            _player.SetAction(PlayerActionType.Jumping);
            
            _currentVerticalSpeed = CalculateJumpSpeed(); 
            _jumpDirection = DetermineJumpDirection();
        }

        private float CalculateJumpSpeed() => Mathf.Sqrt(_jumpPower * GravityScale * Physics.gravity.y);
        
        
        private Vector3 DetermineJumpDirection()
        {
            if (_characterController.velocity.magnitude > 0.1f)
            {
                _animatorHandler.SetBool(AnimatorParameter.JumpRun, true);
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
            
        }
        
        private void UpdateAnimatorParameters()
        {
            _animatorHandler.SetFloat(AnimatorParameter.YSpeed, _currentVerticalSpeed);
        }

        private bool CanJump()
        {
            return _characterController.isGrounded && _player.ActionManager.IsUnoccupied() &&
                   _player.HasEnoughStamina(_player.StaminaCosts.Jump);
        }
        
        #region Events
        public void OnJump(InputAction.CallbackContext context)
        {
            _isJumpTriggered = context.performed;
        }
        
        private void OnVFXPlay()
        {
            AudioManager.PlayRandomAudioOneShot(SoundType.Land);
            VFXManager.Instance.PlayParticleEffect(_player.CharacterEffects.DustVFX, transform.position);
        }
        
        private void OnJumpEnd()
        {
            _player.SetAction(PlayerActionType.Unoccupied);
            _currentVerticalSpeed = 0;
            _animatorHandler.SetBool(AnimatorParameter.JumpRun, false);
        }
        #endregion
    }
}