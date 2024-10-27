namespace RehvidGames.Player
{
    using Interfaces;
    using UnityEngine;
    using Enums;
    using Weapons;

    public class Player: MonoBehaviour, IDamageable
    {
        [SerializeField] private PlayerAttributes _attributes;
        [SerializeField] private Transform _primaryWeaponSocket;
        [SerializeField] private Transform _storageWeaponSocket;
        [SerializeField] private PlayerActionManager _actionManager;
        
        public PlayerActionManager ActionManager => _actionManager;
        
        public PlayerAttributes Attributes => _attributes;
        
        public BaseWeapon Weapon { get; private set; }
        
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
        
        public void ReceiveDamage(float damage, Vector3 hitPosition)
        {
            _attributes.HealthAttribute.ReceiveDamage(damage);
        }

        public bool IsDead()
        {
            return _attributes.HealthAttribute.IsDead();
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