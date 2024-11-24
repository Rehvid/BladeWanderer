namespace RehvidGames.Audio
{
    using Enums;
    using Serializable;
    using UnityEngine;

    public class AudioPlayContext
    {
        public SoundType SoundType { get; }
        public AudioSourceType SourceType { get; }
        public AudioSource AudioSource { get; }
        public AudioClipSettings ClipSettings { get; }
        public float VolumeMultiplier { get; }

        private AudioPlayContext(
            SoundType soundType, 
            AudioSource audioSource, 
            AudioClipSettings clipSettings, 
            float volumeMultiplier,
            AudioSourceType audioSourceType
        )
        {
            SoundType = soundType;
            AudioSource = audioSource ?? throw new System.ArgumentNullException(nameof(audioSource));
            ClipSettings = clipSettings  ?? throw new System.ArgumentNullException(nameof(clipSettings));;
            VolumeMultiplier = volumeMultiplier;
            SourceType = audioSourceType;
        }

        public static AudioPlayContext Create(
            SoundType soundType, 
            AudioSource audioSource, 
            AudioClipSettings clipSettings, 
            float volumeMultiplier,
            AudioSourceType sourceType
        )
        {
            return new AudioPlayContext(soundType, audioSource, clipSettings, volumeMultiplier, sourceType);
        }
    }
}