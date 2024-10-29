namespace RehvidGames.Player
{
    using Animator;
    using Enums;
    using Managers;
    using UnityEngine;
    using UnityEngine.InputSystem;
    using Weapons;

    public class PlayerCombat : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] private Player _player;
        [SerializeField] private AnimatorController _animator;
        
        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Attack();
            }
        }

        public void OnDodge(InputAction.CallbackContext context)
        {
            if (context.performed && _player.ActionManager.IsUnoccupied())
            {
                _animator.SetTrigger(AnimatorParameter.Dodge);
                _animator.ApplyRootMotion();
            }
        }
        
        private void Attack()
        {
            if (!CanAttack()) return; 
            _player.UseStamina(_player.Weapon.Stats.StaminaCost);
            _player.SetAction(PlayerActionType.Attacking);
            
            _animator.SetTrigger(AnimatorParameter.Attack);
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