namespace RehvidGames.Items.Weapons
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "NewWeaponStats", menuName = "Weapons/WeaponStats")]
    public class WeaponStatsData: ScriptableObject
    {
        [SerializeField] private float _baseDamage = 20.0f;
        [SerializeField] private int _baseStaminaCost = 20;

        public float BaseDamage => _baseDamage;
        public int BaseStaminaCost => _baseStaminaCost; 
    }
}