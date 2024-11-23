namespace RehvidGames.Audio
{
    using Enums;
    using Serializable;
    using UnityEngine;

    public class AudioPlayContext
    {
        public SoundType SoundType { get; }
        public AudioSource AudioSource { get; }
        public AudioClipSettings ClipSettings { get; }
        public float VolumeMultiplier { get; }

        private AudioPlayContext(SoundType soundType, AudioSource audioSource, AudioClipSettings clipSettings, float volumeMultiplier)
        {
            SoundType = soundType;
            AudioSource = audioSource ?? throw new System.ArgumentNullException(nameof(audioSource));
            ClipSettings = clipSettings  ?? throw new System.ArgumentNullException(nameof(clipSettings));;
            VolumeMultiplier = volumeMultiplier;
        }

        public static AudioPlayContext Create(SoundType soundType, AudioSource audioSource, AudioClipSettings clipSettings, float volumeMultiplier)
        {
            return new AudioPlayContext(soundType, audioSource, clipSettings, volumeMultiplier);
        }
    }
}