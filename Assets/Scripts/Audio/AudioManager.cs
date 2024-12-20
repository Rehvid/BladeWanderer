﻿namespace RehvidGames.Audio
{
    using System.Collections.Generic;
    using System.Linq;
    using Enums;
    using ScriptableObjects;
    using Serializable;
    using UnityEngine;

    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [Header("Audio components")] 
        [SerializeField] private AudioSourceManager _audioSourceManager;

        [SerializeField] private AudioMixerManager _audioMixerManager;
        [SerializeField] private AudioCollection _audioCollection;

        private Dictionary<SoundType, SoundCategory> _soundCategories;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                InitializeSoundCategories();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void InitializeSoundCategories()
        {
            if (_audioCollection == null)
            {
                Debug.LogError("AudioCollection is not assigned in the AudioManager!");
                return;
            }
            
            _soundCategories = _audioCollection?.SoundCategories.ToDictionary(
                category => category.SoundType,
                category => category
            );
        }
        
        
        public AudioPlayContext GetCurrentAudioPlayContext(SoundType soundType)
        {
            _soundCategories.TryGetValue(soundType, out SoundCategory soundCategory);
            return soundCategory == null ? null : _audioSourceManager.GetActiveAudioContext(soundCategory.AudioSourceType);
        }
        
        public void StopCurrentSoundType(SoundType soundType)
        {
            if (!_soundCategories.TryGetValue(soundType, out SoundCategory soundCategory)) return;
            _audioSourceManager.StopActiveAudioSource(soundCategory.AudioSourceType);   
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
            _audioSourceManager.StopAllActiveAudioSources();
        }

        private void PlayClipInternal(SoundType soundType, bool isRandom, string clipName, AudioSource customAudioSource = null)
        {
            if (!_soundCategories.TryGetValue(soundType, out SoundCategory soundCategory)) return;
            
            if (isRandom && !soundCategory.AllowRandomizeClips) return;

            AudioClipSettings clipSettings = isRandom
                ? soundCategory.GetRandomClipSettings()
                : soundCategory.GetAudioClipSettings(clipName);

            if (clipSettings == null) return;
            
            float volumeMultiplier = _audioMixerManager.GetMixerVolume(soundCategory.AudioSourceType);

            var context = AudioPlayContext.Create(
                soundType,
                _audioSourceManager.GetAudioSourceForClip(soundCategory.AudioSourceType, customAudioSource),
                clipSettings,
                volumeMultiplier,
                soundCategory.AudioSourceType
            );
            
            _audioSourceManager.PlayClip(context);
        }
    }
}