namespace RehvidGames.Characters.Player
{
    using Animator;
    using Audio;
    using Enums;
    using Managers;
    using VFX;
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class PlayerJumpHandler : MonoBehaviour
    {
        [Header("Configuration")]
        private PlayerController _player;
        [SerializeField] private CharacterController _characterController;
        
        [Header("Jump parameters")]
        [SerializeField] private float _jumpPower = 3.0f;
        
        private AnimatorHandler _animatorHandler;
        private bool _isJumpTriggered;
        private float _currentVerticalSpeed;
        private Vector3 _jumpDirection;
        
        private const float GravityScale = -2f;

        private void Start()
        {
            _player = GameManager.Instance.Player;
            if (_player == null)
            {
                Debug.LogWarning($"Player not found");
            }
            
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
            AudioManager.Instance.PlayRandomClip(SoundType.PlayerJump);
            
            _player.UseStamina(_player.StaminaCostsData.BaseJump);
            _player.RegenerationStamina();
            _player.ActionHandler.ChangeCurrentAction(PlayerActionType.Jumping);
            
            _currentVerticalSpeed = CalculateJumpSpeed(); 
            _jumpDirection = DetermineJumpDirection();
        }

        private float CalculateJumpSpeed() => Mathf.Sqrt(_jumpPower * GravityScale * Physics.gravity.y);
        
        
        private Vector3 DetermineJumpDirection()
        {
            if (_characterController.velocity.magnitude > 0.1f)
            {
                var forwardJumpDistance = new Vector3(_characterController.velocity.x, 0, _characterController.velocity.z).magnitude;
                _animatorHandler.SetBool(AnimatorParameter.JumpRun, true);
                return new Vector3(_characterController.velocity.x, 0, _characterController.velocity.z).normalized * forwardJumpDistance;
            }
            return Vector3.zero; 
        }

        private void ApplyJumpMovement()
        {
            if (!_player.ActionHandler.IsJumping()) return;
          
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
            return _characterController.isGrounded && _player.ActionHandler.IsUnoccupied() &&
                   _player.HasEnoughStamina(_player.StaminaCostsData.BaseJump);
        }
        
        #region Events
        public void OnJumpPerformed(InputAction.CallbackContext context)
        {
            _isJumpTriggered = context.performed;
        }
        
        private void OnVFXPlay()
        {
            AudioManager.Instance.PlayRandomClip(SoundType.PlayerLand);
            VFXManager.Instance.PlayParticleEffect(_player.CharacterEffectsData.DustGroundVFX, transform.position);
        }
        
        private void OnJumpEnd()
        {
            _player.ActionHandler.ChangeCurrentAction(PlayerActionType.Unoccupied);
            _currentVerticalSpeed = 0;
            _animatorHandler.SetBool(AnimatorParameter.JumpRun, false);
        }
        #endregion
    }
}