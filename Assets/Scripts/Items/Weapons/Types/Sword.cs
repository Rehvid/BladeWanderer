namespace RehvidGames.Items.Weapons.Types
{
    using Base;
    using Interfaces;
    using Animator;
    using Characters.Player;
    using Enums;
    using RehvidGames.Interfaces;
    using UnityEngine;
    
    public class Sword: BaseWeapon, IPickUpWeapon
    {
        public bool CanInteract(PlayerController player) => player != null && !player.IsWeaponEquipped();
        
        public Collider GetPickableWeaponCollider() => TryGetComponent(out SphereCollider sphereCollider) ? sphereCollider : null;
        
        public void Interact(PlayerController player)
        {
            if (player.IsWeaponEquipped()) return;
            
            SetInteractingAction(player);
            SetInteractionAnimation(player);
            DestroyPickableWeaponCollider();
        }

        private void OnDestroy()
        {
            Destroy(GetWaveMotionController());
        }

        private void SetInteractionAnimation(PlayerController player)
        {
            AnimatorHandler animator = player.AnimatorHandler;
            
            animator.SetTrigger(AnimatorParameter.Interaction);
            animator.SetTrigger(AnimatorParameter.PickUp);
            animator.SetBool(AnimatorParameter.HasEquippedWeapon, true);
        }
        
        private void DestroyPickableWeaponCollider()
        {
            var interactableCollider = GetPickableWeaponCollider();
            if (interactableCollider)
            {
                Destroy(interactableCollider);   
            }
        }

        private GameObject GetWaveMotionController()
        {
            var waveMotionController = gameObject.transform.parent;
            return waveMotionController.CompareTag("WaveMotionVFX") ? waveMotionController.gameObject : null;
        }
        
        public void PickUp(PlayerController player)
        {
            GameObject waveMotionController = GetWaveMotionController();
            waveMotionController?.SetActive(false);
            
            SetInteractingAction(player); 
            
            player.WeaponHandler.AttachWeaponToRightHand(this);

            StartCoroutine(player.AnimatorHandler.WaitForCurrentAnimationThenInvoke(() =>
            {
                Destroy(waveMotionController);
                player.ActionHandler.ChangeCurrentAction(PlayerActionType.Unoccupied);
            }));
        }
        
        private void SetInteractingAction(PlayerController player) 
            => player.ActionHandler.ChangeCurrentAction(PlayerActionType.Interacting);
        
        protected override void OnTriggerEnter(Collider otherCollider)
        {
            if (GetPickableWeaponCollider()) return; 
            
            base.OnTriggerEnter(otherCollider);
        }
    }
}