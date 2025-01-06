namespace RehvidGames.Characters.Player
{
    using Items.Weapons.Interfaces;
    using Interfaces;
    using Managers;
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class PlayerInteraction: MonoBehaviour
    {
        private IInteractable _interactableObject;
        private PlayerController _player; 
        
        private void Start()
        {
            _player = GameManager.Instance.Player;
            if (_player == null)
            {
                Debug.LogWarning("No player found!");
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            other.TryGetComponent(out _interactableObject);
        }
        
        public void OnInteractionPerformed(InputAction.CallbackContext context)
        {
            if (context.performed && CanInteract())
            {
                _interactableObject.Interact(_player);
            }
        }
        
        private bool CanInteract() => _interactableObject != null && _interactableObject.CanInteract(_player);
        
        private void OnTriggerExit(Collider other)
        {
            if (IsInteractableObject(other))
            {
                _interactableObject = null;
            }
        }

        private bool IsInteractableObject(Collider other) =>
            other.TryGetComponent<IInteractable>(out var interactable) && interactable == _interactableObject;
        
        public void OnPickUpWeapon()
        {
            if (_interactableObject is not IPickUpWeapon weapon) return;
            weapon.PickUp(_player);
        }
    }
}