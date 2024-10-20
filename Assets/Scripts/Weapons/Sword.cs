namespace RehvidGames.Weapons
{
    using System;
    using Animator;
    using Behaviors;
    using Data.Serializable;
    using Enums;
    using Player;
    using UnityEngine;
    using UnityEngine.Events;

    public class Sword: BaseWeapon
    {
        [SerializeField] private WaveMotionController _waveMotionController;
        [SerializeField] private UnityEvent<AnimationData[]> _interactionAnimations;
        
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
                _waveMotionController.active = false;
                Destroy(_waveMotionController);
            }
        }
        
        public override void Interact(Player player)
        {
            if (player.Weapon != null) {return;}
            
            if (_interactionAnimations != null)
            { 
                player.SetAction(PlayerActionType.Interacting);
                RaiseInteractMultiAnimationsEvent();
            }
            
            if (TryGetComponent(out SphereCollider sphereCollider))
            {
              Destroy(sphereCollider);   
            }
        }

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

        private void RaiseInteractMultiAnimationsEvent()
        {
            const AnimatorParameterType type = AnimatorParameterType.Trigger;
            AnimationData[] animations = {
                new()
                {
                    AnimationName = AnimatorParameter.GetParameterName(AnimatorParameter.Interaction), 
                    ParameterType = type
                },
                new()
                {
                    AnimationName = AnimatorParameter.GetParameterName(AnimatorParameter.PickUp), 
                    ParameterType = type
                },
                new()
                {
                    AnimationName = AnimatorParameter.GetParameterName(AnimatorParameter.HasEquippedWeapon),
                    ParameterType = AnimatorParameterType.Bool,
                    Value = true
                }
            };
            _interactionAnimations?.Invoke(animations); 
        }
    }
}