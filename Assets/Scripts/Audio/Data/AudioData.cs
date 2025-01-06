namespace RehvidGames.Audio.Data
{
    using Enums;
    using UnityEngine;

    [CreateAssetMenu(fileName = "NewAudioData", menuName = "Sound/AudioData")]
    public class AudioData : ScriptableObject
    {
        [Header("Sound")]
        [SerializeField] private string _soundName;
        [SerializeField] private SoundType _soundType;
        
        public string SoundName => _soundName;
        
        public SoundType SoundType => _soundType;
    }
}