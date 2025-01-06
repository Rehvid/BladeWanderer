namespace RehvidGames.Characters.Base
{
    using Items.Weapons.Base;
    using UnityEngine;

    public abstract class BaseCharacterWeaponHandler: MonoBehaviour
    {
        [Header("Weapon")]
        [SerializeField] protected BaseWeapon _weapon;
        
        public BaseWeapon CurrentWeapon => _weapon;

        #region Events
        private void OnWeaponEnableDamageCollider() => _weapon?.EnableDamageCollider();

        private void OnWeaponDisableDamageCollider() => _weapon?.DisableDamageCollider();
        
        private void OnWeaponEffects() => _weapon?.PlayEffects();
        #endregion
    }
}