namespace RehvidGames.Weapons
{
    using UnityEngine;

    [System.Serializable]
    public class WeaponStats
    {
        [SerializeField] private float _damage = 20.0f;
        [SerializeField] private int _staminaCost = 20;

        public float Damage => _damage;
        public int StaminaCost => _staminaCost; 
    }
}