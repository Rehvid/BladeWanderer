namespace RehvidGames.Interfaces
{
    using UnityEngine;
    using Weapons;

    public interface IWeapon
    {
        public WeaponStats Stats { get; }
        
        public bool IsCurrentlyEquipped { get; }
        
        public void SetCurrentlyEquipped(bool value);
        
        public void EnableDamageCollider();
        
        public void DisableDamageCollider();
        
        public void Equip(Transform socket);
        
        public void UnEquip(Transform socket);
    }
}