namespace RehvidGames.Audio
{
    using Enums;
    using Serializable;
    using UnityEngine;

    public class AudioSourceManager : MonoBehaviour
    {
        [Header("Audio sources")]
        [SerializeField] private AudioSource _sfxSource;
        [SerializeField] private AudioSource _musicSource;

        private AudioSource _currentSource;

        public void StopCurrentClip() => _currentSource?.Stop();
        
        public void PlayClip(
            AudioSourceType sourceType, 
            AudioClipSettings clipSettings, 
            float volumeMultiplier,
            AudioSource customAudioSource = null
        )
        {
            var source = GetAudioSourceForClip(sourceType, customAudioSource);
            if (source == null) return;
            
            StopCurrentClip();
            ConfigureAudioSource(source, clipSettings, volumeMultiplier);
            if (clipSettings.Loop)
            {
                source.Play();
            }
            else
            {
                source.PlayOneShot(clipSettings.Clip);
            }
        }        
        
        private AudioSource GetAudioSourceForClip(AudioSourceType audioSourceType, AudioSource customAudioSource = null)
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

        private void ConfigureAudioSource(AudioSource source, AudioClipSettings clipSettings, float volumeMultiplier)
        {
            source.volume = clipSettings.Volume * volumeMultiplier;
            source.pitch = clipSettings.Pitch;
            source.loop = clipSettings.Loop;

            if (source.loop)
            {
                source.clip = clipSettings.Clip;
            }
            
            _currentSource = source;
        }
    }
}