namespace RehvidGames.Characters.Base
{
    using Audio.Data;
    using VFX;
    using UnityEngine;

    public abstract class BaseCharacterEffectsData : ScriptableObject
    {
        [Header("General Character Effects")]
        [SerializeField] private VFXConfig _hitTakenBloodVfx;

        [Header("General Character Sounds")] 
        [SerializeField] private AudioData _combatVoice;
        
        public VFXConfig HitTakenBloodVfx => _hitTakenBloodVfx;
        public AudioData CombatVoice => _combatVoice;
    }
}