namespace RehvidGames.VFX
{
    using UnityEngine;

    public enum VFXType
    {
        ParticleEffect,
        VisualEffect
    }
 
    [CreateAssetMenu(fileName = "NewVFXConfig", menuName = "VFX/VFXConfig")]
    public class VFXConfig : ScriptableObject
    {
        [Header("VFX Settings")]
        public VFXType VfxType;
        public float Duration = 1;
        
        [Header("VFX Prefab")]
        public GameObject EffectPrefab;
    }
}