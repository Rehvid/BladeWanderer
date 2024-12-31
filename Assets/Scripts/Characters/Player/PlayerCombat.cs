namespace RehvidGames.Characters.Player
{
    using Animator;
    using Audio;
    using Audio.Data;
    using Enums;
    using Managers;
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class PlayerCombat: MonoBehaviour
    {
        private PlayerController _player;
        private AnimatorHandler _animatorHandler;
        private AudioManager _audioManager;
        
        private void Start()
        {
            _player = GameManager.Instance.Player;
            if (_player == null)
            {
                Debug.LogWarning($"Player not found");
            }
            
            _animatorHandler = _player.AnimatorHandler;
            _audioManager = AudioManager.Instance;
        }
        
        public void OnAttackPerformed(InputAction.CallbackContext context)
        {
            if (context.performed && CanAttack())
            {
                Attack();
            }
        }
        
        private bool CanAttack()
        {
            return _player.IsWeaponEquipped() 
                   && _player.IsWeaponDrawn() 
                   && _player.ActionHandler.IsUnoccupied() 
                   &&_player.HasEnoughStamina(StaminaCostForAttack());
        }

        private void Attack()
        {
            _player.ActionHandler.ChangeCurrentAction(PlayerActionType.Attacking);
            
            _player.UseStamina(StaminaCostForAttack());
            _player.RegenerationStamina();
            
            _animatorHandler.SetTrigger(AnimatorParameter.Attack);

            AudioData combatVoice = _player.CharacterEffectsData.CombatVoice;
            _audioManager.PlayClip(combatVoice.SoundType, combatVoice.SoundName);
        }
        
        private float StaminaCostForAttack() => _player.WeaponHandler.CurrentWeapon.StatsData.BaseStaminaCost;
        
        
        public void OnDodgePerformed(InputAction.CallbackContext context)
        {
            if (context.performed || CanDodge())
            {
                Dodge();
            }
        }
        
        private bool CanDodge() => _player.ActionHandler.IsUnoccupied() && _player.HasEnoughStamina(_player.StaminaCostsData.BaseDodge);

        private void Dodge()
        {
            _audioManager.StopCurrentSoundType(SoundType.PlayerFootsteps); 
            _animatorHandler.SetTrigger(AnimatorParameter.Dodge);
            
            _player.ActionHandler.ChangeCurrentAction(PlayerActionType.Dodge);
            _player.UseStamina(_player.StaminaCostsData.BaseDodge);
            _player.RegenerationStamina();
        }
    }
}