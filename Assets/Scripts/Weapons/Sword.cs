namespace RehvidGames.Weapons
{
    using Animator;
    using Enums;
    using Helpers;
    using Interfaces;
    using Player;
    using UnityEngine;
   

    public class Sword: BaseWeapon, IInteractable
    {
        [SerializeField] private WaveMotionController _waveMotionController;
        
        public void Interact(Player player)
        {
            if (player.Weapon != null) return;
            AnimatorHandler animator = player.AnimatorHandler;
            
            
            player.SetAction(PlayerActionType.Interacting);
            animator.SetTrigger(AnimatorParameter.Interaction);
            animator.SetTrigger(AnimatorParameter.PickUp);
            animator.SetBool(AnimatorParameter.HasEquippedWeapon, true);
            
            if (TryGetComponent(out SphereCollider sphereCollider))
            {
               Destroy(sphereCollider);   
            }

            if (!TryGetComponent(out WaveMotionController waveMotionController)) return;
            waveMotionController.enabled = false;
            Destroy(waveMotionController);
        }

        public bool CanInteract(Player player) => player != null && player.Weapon == null;
    }
}