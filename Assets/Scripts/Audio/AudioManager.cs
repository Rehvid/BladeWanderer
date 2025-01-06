namespace RehvidGames.Audio
{
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using Enums;
    using Serializable;
    using Service;
    using UnityEngine;
    using Utilities;

    public class AudioManager : BaseSingletonMonoBehaviour<AudioManager>
    {
        [Header("Audio components")] 
        [SerializeField] private AudioSourceService audioSourceService;
        [SerializeField] private AudioMixerService audioMixerService;
        [SerializeField] private AudioCollectionData audioCollectionData;

        private Dictionary<SoundType, SoundCategory> _soundCategories;
        
        protected override void Awake()
        {
            base.Awake();
            InitializeSoundCategories();
        }

        private void InitializeSoundCategories()
        {
            if (audioCollectionData == null)
            {
                Debug.LogError("AudioCollection is not assigned in the AudioManager!");
                return;
            }
            
            _soundCategories = audioCollectionData?.SoundCategories.ToDictionary(
                category => category.SoundType,
                category => category
            );
        }
        
        
        public AudioPlayContext GetCurrentAudioPlayContext(SoundType soundType)
        {
            _soundCategories.TryGetValue(soundType, out SoundCategory soundCategory);
            return soundCategory == null ? null : audioSourceService.GetActiveAudioContext(soundCategory.AudioSourceType);
        }
        
        public void StopCurrentSoundType(SoundType soundType)
        {
            if (!_soundCategories.TryGetValue(soundType, out SoundCategory soundCategory)) return;
            audioSourceService.StopActiveAudioSource(soundCategory.AudioSourceType);   
        }
        
        public void PlayClip(SoundType soundType, string clipName, AudioSource customAudioSource = null)
        {
            PlayClipInternal(soundType, isRandom: false, clipName, customAudioSource);
        }

        public void PlayRandomClip(SoundType soundType, AudioSource customAudioSource = null)
        {
            PlayClipInternal(soundType, isRandom: true, clipName: null, customAudioSource);
        }

        public void StopAllSounds()
        {
            audioSourceService.StopAllActiveAudioSources();
        }

        private void PlayClipInternal(SoundType soundType, bool isRandom, string clipName, AudioSource customAudioSource = null)
        {
            if (!_soundCategories.TryGetValue(soundType, out SoundCategory soundCategory)) return;
            
            if (isRandom && !soundCategory.AllowRandomizeClips) return;

            AudioClipSettings clipSettings = isRandom
                ? soundCategory.GetRandomClipSettings()
                : soundCategory.GetAudioClipSettings(clipName);

            if (clipSettings == null) return;
            
            float volumeMultiplier = audioMixerService.GetMixerVolume(soundCategory.AudioSourceType);

            var context = AudioPlayContext.Create(
                soundType,
                audioSourceService.GetAudioSourceForClip(soundCategory.AudioSourceType, customAudioSource),
                clipSettings,
                volumeMultiplier,
                soundCategory.AudioSourceType
            );
            
            audioSourceService.PlayClip(context);
        }
    }
}