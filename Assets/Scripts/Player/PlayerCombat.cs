namespace RehvidGames.Player
{
    using Animator;
    using Data.Serializable;
    using Enums;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.InputSystem;

    public class PlayerCombat : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField] private UnityEvent<AnimationData> attackTriggered;
        
        [Header("Configuration")]
        [SerializeField] private Player _player;
        
        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Attack();
            }
        }
        
        private void Attack()
        {
            if (!CanAttack()) return; 
            _player.UseStamina(_player.Weapon.Stats.StaminaCost);
            _player.SetAction(PlayerActionType.Attacking);
            attackTriggered.Invoke(PrepareAnimationDataForAttack());
        }

        private AnimationData PrepareAnimationDataForAttack()
        {
            return new AnimationData
            {
                AnimationName = AnimatorParameter.GetParameterName(AnimatorParameter.Attack),
                ParameterType = AnimatorParameterType.Trigger
            };
        }
        
        private bool CanAttack()
        {
            return _player.HasEquippedWeapon()
                   && _player.ActionManager.IsUnoccupied()
                   && _player.HasEnoughStamina(_player.Weapon.Stats.StaminaCost);
            ;
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