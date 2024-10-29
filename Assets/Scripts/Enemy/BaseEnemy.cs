namespace RehvidGames.Enemy
{
    using Weapons;
    using Character;
    using UnityEngine;

    public abstract class BaseEnemy: BaseCharacter
    {
        [SerializeField] private BaseWeapon _weapon;
        [SerializeField] protected GameObject _treasure;
        
        private void Start()
        {
            Weapon = _weapon;
        }
        
        protected abstract void HandleDeath();
        public abstract void OnDeath(Component sender, object value = null);
    }
}