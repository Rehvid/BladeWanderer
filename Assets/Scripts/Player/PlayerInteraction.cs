namespace RehvidGames.Player
{
    using Animator;
    using Enums;
    using Interfaces;
    using UnityEngine;
    using UnityEngine.InputSystem;
    using Weapons;

    public class PlayerInteraction: MonoBehaviour
    {
        private IInteractable _interactableObject;
        [SerializeField] private Player _player;
        
        
        public void OnInteraction(InputAction.CallbackContext context)
        {
            if (context.performed && CanInteract())
            {
                _interactableObject.Interact(_player);
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            other.TryGetComponent(out _interactableObject);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<IInteractable>(out var interactable) && interactable == _interactableObject)
            {
                _interactableObject = null;
            }
        }

        private bool CanInteract()
        {
            return _interactableObject != null && _interactableObject.CanInteract(_player);
        }
        
        public void OnWeaponToggle(InputAction.CallbackContext context) 
        {
            if (!context.performed || _player.Weapon == null) return; 
            var weapon = _player.Weapon;
            
            if (weapon.IsCurrentlyEquipped)
            {
                SheatheWeapon();
            }
            else
            {
                DrawWeapon();
            }
        }
        
        public void OnPickUpWeapon()
        {
            if (_interactableObject is not BaseWeapon weapon) return;
            GameObject parentWeapon = weapon.transform.parent.gameObject;
            
            _player.SetAction(PlayerActionType.Interacting);
            _player.AttachWeaponToPrimarySocket(weapon);
            
            if (parentWeapon != null && parentWeapon.CompareTag("Temporary"))
            {
                Destroy(parentWeapon);
            }
            
            _player.SetAction(PlayerActionType.Unoccupied);
        }
        
        private void SheatheWeapon()
        {
            _player.SetAction(PlayerActionType.Interacting);
            RaiseInteractMultiAnimationsForToggleWeapon(false);
        }
        
        private void OnSheathingWeaponEnd()
        {
            _player.AttachWeaponToStorageSocket();
            _player.SetAction(PlayerActionType.Unoccupied);
        }
        
        private void DrawWeapon()
        {
            _player.SetAction(PlayerActionType.Interacting);
            RaiseInteractMultiAnimationsForToggleWeapon(true);
        }
        
        private void OnDrawingWeaponStart()
        {
            _player.AttachWeaponToPrimarySocket(_player.Weapon);
        }
        
        private void OnDrawingWeaponEnd()
        {
            _player.SetAction(PlayerActionType.Unoccupied); 
        }
        
        private void RaiseInteractMultiAnimationsForToggleWeapon(bool isDrawingWeapon)
        {
            _player.AnimatorHandler.SetTrigger(AnimatorParameter.Interaction);
            _player.AnimatorHandler.SetTrigger( isDrawingWeapon ? AnimatorParameter.DrawWeapon : AnimatorParameter.HideWeapon);
            _player.AnimatorHandler.SetBool(AnimatorParameter.HasEquippedWeapon, isDrawingWeapon);
        }
    }
}