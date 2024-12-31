namespace RehvidGames.Characters.Player.Data
{
    using Base;
    using VFX;
    using UnityEngine;

    [CreateAssetMenu(fileName = "NewPlayerCharacterEffectsData", menuName = "VFX/PlayerCharacterEffects")]
    public class PlayerCharacterEffectsData : BaseCharacterEffectsData
    {
        [Header("Player Specific Effects")]
        [SerializeField] private VFXConfig _dustGroundVFX;
        
        public VFXConfig DustGroundVFX => _dustGroundVFX;
    }
}