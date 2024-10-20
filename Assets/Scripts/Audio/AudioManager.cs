namespace RehvidGames.Audio
{
    using System;
    using System.Linq;
    using Enums;
    using UnityEngine;
    using Random = UnityEngine.Random;

    public class AudioManager: MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [SerializeField] private AudioSettings _audioSettings;
        [SerializeField] private AudioSource _sfxSource;
        [SerializeField] private AudioSource _musicSource;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }
        
        public static void PlayRandomAudioOneShot(SoundType audioType, AudioSource source = null)
        {
            if (!Instance._audioSettings) return;
            AudioCollection audioCollection = GetAudioCollectionByType(audioType);
            AudioClipEntry[] clips = GetAudioClipsEntryFromAudioCollection(audioCollection);
            if (clips == null) return;
            
            AudioClipEntry randomClipEntry = clips[Random.Range(0, clips.Length)];
            AudioSource currentAudioSource = GetAudioSourceFromAudioCollection(audioCollection);
            
            if (source != null)
            {
                source.PlayOneShot(randomClipEntry.Clip);
            }
            else
            {
                currentAudioSource.PlayOneShot(randomClipEntry.Clip);
            }
        }

        public static void PlayAudioOneShot(SoundType soundType, string clipName, AudioSource source = null)
        {
            if (!Instance._audioSettings) return;
            AudioCollection audioCollection = GetAudioCollectionByType(soundType);
            AudioClipEntry[] clips = GetAudioClipsEntryFromAudioCollection(audioCollection);
            if (clips == null) return;
            
            AudioSource currentAudioSource = GetAudioSourceFromAudioCollection(audioCollection);

            var audioClipEntry = clips.FirstOrDefault(audioClipEntry => audioClipEntry.ClipName == clipName);
            if (audioClipEntry != null)
            {
                currentAudioSource.PlayOneShot(audioClipEntry.Clip);
            }
        }
        
        private static AudioCollection GetAudioCollectionByType(SoundType type)
        {
            return Instance._audioSettings.AudioCollections[(int)type];
        }

        private static AudioClipEntry[] GetAudioClipsEntryFromAudioCollection(AudioCollection audioCollection)
        {
            AudioClipEntry[] clips = audioCollection.Clips;
            return clips.Length <= 0 ? null : clips;
        }

        private static AudioSource GetAudioSourceFromAudioCollection(AudioCollection audioCollection)
        {
            return audioCollection.AudioSourceType switch
            {
                AudioSourceType.Music => Instance._musicSource,
                AudioSourceType.SFX => Instance._sfxSource,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}