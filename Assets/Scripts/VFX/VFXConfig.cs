namespace RehvidGames.VFX
{
    using Enums;
    using UnityEngine;
    
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