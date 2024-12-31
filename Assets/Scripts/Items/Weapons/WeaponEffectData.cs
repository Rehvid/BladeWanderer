namespace RehvidGames.Items.Weapons
{
    using Audio;
    using Audio.Data;
    using VFX;
    using UnityEngine;

    [CreateAssetMenu(fileName = "NewWeaponEffect", menuName = "Weapons/WeaponEffect")]
    public class WeaponEffectData : ScriptableObject
    {
        [Header("Sound")]
        [SerializeField] private AudioData _audioData;
        
        [Header("Visual Effect")]
        [SerializeField] private VFXConfig _visualEffect;
        
        public AudioData AudioData => _audioData;
        
        public VFXConfig VisualEffect => _visualEffect;
    }
}