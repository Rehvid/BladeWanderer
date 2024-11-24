namespace RehvidGames.Player
{
    using Animator;
    using Audio;
    using Enums;
    using Managers;
    using UnityEngine;
    using UnityEngine.InputSystem;
    using Weapons;

    public class PlayerCombat : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] private Player _player;
        
        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Attack();
            }
        }

        public void OnDodge(InputAction.CallbackContext context)
        {
            if (!context.performed || !CanDodge()) return;
            AudioManager.Instance.StopCurrentSoundType(SoundType.PlayerFootsteps); //TODO: Refactor later
            _player.AnimatorHandler.SetTrigger(AnimatorParameter.Dodge);
            _player.SetAction(PlayerActionType.Dodge);
            _player.UseStamina(_player.StaminaCosts.Dodge);
            _player.RegenerationStamina();
        }

        public void OnDodgeEnd() => _player.SetAction(PlayerActionType.Unoccupied);

        private bool CanDodge() => _player.ActionManager.IsUnoccupied() && _player.HasEnoughStamina(_player.StaminaCosts.Dodge);
        
        private void Attack()
        {
            if (!CanAttack()) return; 
            _player.SetAction(PlayerActionType.Attacking);
            _player.UseStamina(_player.Weapon.Stats.StaminaCost);
            _player.RegenerationStamina();
            _player.AnimatorHandler.SetTrigger(AnimatorParameter.Attack);
            AudioManager.Instance.PlayClip(SoundType.PlayerVoice, "PlayerVoiceCombat");
        }
        
        private bool CanAttack()
        {
            return _player.HasEquippedWeapon()
                   && _player.ActionManager.IsUnoccupied()
                   && _player.HasEnoughStamina(_player.Weapon.Stats.StaminaCost);
            ;
        }

        private void OnPlayVFX()
        {
            var weapon = _player.Weapon;
            if (weapon is Sword sword)
            {
                VFXManager.Instance.PlayVisualEffect(_player.CharacterEffects.SlashSwordVfx, sword.transform.position);
                AudioManager.Instance.PlayClip(SoundType.WeaponSwing, "WeaponSwingSword");
            }
        }
        
        private void OnAttackEnd()
        {
            _player.SetAction(PlayerActionType.Unoccupied);
        }
        
        private void OnEnableCollision()
        {
            _player.Weapon.EnableDamageCollider();
        }

        private void OnDisableCollision()
        {
            _player.Weapon.DisableDamageCollider();
        }
    }
}