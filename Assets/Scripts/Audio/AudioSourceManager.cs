namespace RehvidGames.Audio
{
    using System.Collections.Generic;
    using Enums;
    using Serializable;
    using UnityEngine;

    public class AudioSourceManager : MonoBehaviour
    {
        [Header("Audio sources")]
        [SerializeField] private AudioSource _sfxSource;
        [SerializeField] private AudioSource _musicSource;
        
        private Dictionary<AudioSource, AudioPlayContext> _activeAudioSources = new();

        public AudioPlayContext GetActiveAudioContext(AudioSourceType audioSourceType)
        {
            return !_activeAudioSources.TryGetValue(GetAudioSource(audioSourceType), out AudioPlayContext context) ? null : context;
        }
        
        public void StopActiveAudioSource(AudioSourceType sourceType)
        {
            if (!_activeAudioSources.TryGetValue(GetAudioSource(sourceType), out AudioPlayContext audioPlayContext)) return;
            
            audioPlayContext.AudioSource.Stop();
            _activeAudioSources.Remove(audioPlayContext.AudioSource);
        }
        
        public void PlayClip(AudioPlayContext audioPlayContext)
        {
            var source = audioPlayContext.AudioSource;
            StopActiveAudioSource(audioPlayContext.SourceType);
            ConfigureAudioSource(audioPlayContext);
            
            if (audioPlayContext.ClipSettings.Loop)
            {
                source.Play();
            }
            else
            {
                source.PlayOneShot(audioPlayContext.ClipSettings?.Clip);
            }
        }        
        
        public AudioSource GetAudioSourceForClip(AudioSourceType audioSourceType, AudioSource customAudioSource = null)
        {
            return customAudioSource ? customAudioSource : GetAudioSource(audioSourceType);
        }

        private AudioSource GetAudioSource(AudioSourceType audioSourceType) 
        {
            return audioSourceType switch
            {
                AudioSourceType.Music => _musicSource,
                AudioSourceType.SFX => _sfxSource,
                _ => null
            };
        }

        private void ConfigureAudioSource(AudioPlayContext audioPlayContext)
        {
            AudioSource source = audioPlayContext.AudioSource;
            AudioClipSettings clipSettings = audioPlayContext.ClipSettings;
            
            source.volume = clipSettings.Volume * audioPlayContext.VolumeMultiplier;
            source.pitch = clipSettings.Pitch;
            source.loop = clipSettings.Loop;

            if (source.loop)
            {
                source.clip = clipSettings.Clip;
            }
            
            _activeAudioSources[source] = audioPlayContext;
        }
    }
}