namespace RehvidGames.ScriptableObjects
{
    using UnityEngine;

    public enum VFXType
    {
        ParticleEffect,
        VisualEffect
    }
    
    [CreateAssetMenu(menuName = "VFX/VFX Config")]
    public class VFXConfig : ScriptableObject
    {
        public VFXType VfxType;
        public GameObject EffectPrefab;
        public float Duration = 1;
    }
}