namespace RehvidGames.Weapons
{
    using Animator;
    using Enums;
    using Helpers;
    using Player;
    using UnityEngine;
   

    public class Sword: BaseWeapon
    {
        [SerializeField] private WaveMotionController _waveMotionController;
        [SerializeField] private AnimatorHandler _animator;
        
        private void Update()
        {
            Wave();
        }

        private void Wave()
        {
            if (!_waveMotionController) return;
            
            if (IsCurrentlyEquipped == false)
            {
                _waveMotionController.active = true;
            }
            else
            {
                Destroy(_waveMotionController);
                
            }
        }
        
        public override void Interact(Player player)
        {
            if (player.Weapon != null) {return;}
            
            player.SetAction(PlayerActionType.Interacting);
            _animator.SetTrigger(AnimatorParameter.Interaction);
            _animator.SetTrigger(AnimatorParameter.PickUp);
            _animator.SetBool(AnimatorParameter.HasEquippedWeapon, true);
            
            if (TryGetComponent(out SphereCollider sphereCollider))
            {
               Destroy(sphereCollider);   
            }
        }

        public override bool CanInteract(Player player) => player != null && player.Weapon == null;

        public override void EnableDamageCollider()
        {
            damageCollider.enabled = true;
            damageCollider.isTrigger = true;
        }

        public override void DisableDamageCollider()
        {
            damageCollider.enabled = false;
            damageCollider.isTrigger = false;
        }
        
    }
}