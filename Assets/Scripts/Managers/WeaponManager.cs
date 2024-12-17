namespace RehvidGames.Managers
{
    using System.Collections.Generic;
    using UnityEngine;
    using Weapons;

    public class WeaponManager : MonoBehaviour
    {
        [SerializeField] private Transform _weaponHolder;
        [SerializeField] private Transform _storageHolder;
        
        private readonly List<BaseWeapon> _weaponPrefabs = new();

        private void Awake()
        { 
            BaseWeapon[] weapons = Resources.LoadAll<BaseWeapon>("Weapons");
            if (weapons.Length > 0)
            {
                _weaponPrefabs.AddRange(weapons); 
            }
        }

        public BaseWeapon InstantiateWeapon(string weaponName, Transform socket)
        {
            BaseWeapon weaponToEquip = _weaponPrefabs.Find(weapon => weapon.Name == weaponName);
            if (!weaponToEquip) return null;
            var weapon = Instantiate(weaponToEquip);
            weapon.Equip(socket);
            
            return weapon;
        }
        
    }
}