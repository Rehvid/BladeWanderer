namespace RehvidGames.Player
{
    using Animator;
    using Character;
    using UnityEngine;
    using Enums;
    using Weapons;

    public class Player: BaseCharacter
    {
        [Header("Player configuration")]
        [SerializeField] private PlayerAttributes _attributes;
        [SerializeField] private Transform _primaryWeaponSocket;
        [SerializeField] private Transform _storageWeaponSocket;
        [SerializeField] private PlayerActionManager _actionManager;
        
        public PlayerActionManager ActionManager => _actionManager;
        
        public PlayerAttributes Attributes => _attributes;

        public Transform PrimaryWeaponSocket => _primaryWeaponSocket;
        
        public override void ReceiveDamage(float damage, Vector3 hitPosition)
        {
            healthAttribute.ReceiveDamage(damage, hitPosition);
            HitDirectionType directionType = hitDirectionAnalyzer.GetDirectionType(hitPosition, transform);
            // VFXManager.Instance.PlayParticleEffect(CharacterEffects.HitVfx, hitPosition);
            animatorController.SetTrigger(AnimatorParameter.HitDirection);
            animatorController.SetTrigger(hitDirectionAnalyzer.GetAnimatorParameterTypeByHitDirectionType(directionType));
        }
        
        public bool HasEquippedWeapon()
        {
            return Weapon != null && Weapon.IsCurrentlyEquipped;
        }
        
        public void AttachWeaponToPrimarySocket(BaseWeapon weapon)
        {
            Weapon = weapon;
            Weapon.Equip(_primaryWeaponSocket);
        }
        
        public void AttachWeaponToStorageSocket()
        {
            Weapon.UnEquip(_storageWeaponSocket);
        }
        
        public void SetAction(PlayerActionType actionType)
        {
            _actionManager.ChangeCurrentAction(actionType);
        }
        
        public bool HasEnoughStamina(float staminaCost)
        {
            return _attributes.StaminaAttribute.HasEnoughStamina(staminaCost);
        }

        public void UseStamina(float staminaCost)
        {
            _attributes.StaminaAttribute.UseStamina(staminaCost);
        }
        
        public void OnDeath(Component sender, object value = null)
        {
            Debug.Log("OnDeath - Player"); 
        }
    }
}