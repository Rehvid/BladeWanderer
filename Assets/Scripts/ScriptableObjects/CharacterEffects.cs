namespace RehvidGames.ScriptableObjects
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "VFX/CharacterEffects")]
    public class CharacterEffects : ScriptableObject
    {
        public VFXConfig HitVfx;
        public VFXConfig SlashSwordVfx;
        public VFXConfig DustVFX;
    }
}