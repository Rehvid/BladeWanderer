namespace RehvidGames.Audio.Serializable
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Enums;
    using Random = UnityEngine.Random;

    [Serializable]
    public class SoundCategory
    {
        public SoundType SoundType;            
        public AudioSourceType AudioSourceType;   
        public bool AllowRandomizeClips;  
        public List<AudioClipSettings> Clips;  
        
        public AudioClipSettings GetAudioClipSettings(string clipName)
        {
            return Clips.FirstOrDefault(clip => clip.Name.Equals(clipName, StringComparison.OrdinalIgnoreCase));
        }
        
        public AudioClipSettings GetRandomClipSettings()
        {
            if (Clips == null || Clips.Count == 0) return null;
            return Clips[Random.Range(0, Clips.Count)];
        }
    }
}