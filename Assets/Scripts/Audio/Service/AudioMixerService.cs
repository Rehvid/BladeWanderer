namespace RehvidGames.Audio.Service
{
    using System;
    using RehvidGames.Enums;
    using UnityEngine;
    using UnityEngine.Audio;

    public class AudioMixerService : MonoBehaviour
    {
        private const float DefaultVolume = 1.0f;
        
        [SerializeField] private AudioMixer _audioMixer;
        
        public float GetMixerVolume(AudioSourceType audioSourceType)
        {
            string parameterName = GetParameterName(audioSourceType);
            if (string.IsNullOrEmpty(parameterName)) return DefaultVolume;
        
            return _audioMixer.GetFloat(parameterName, out float volumeInDb) 
                ? TransformVolumeDBIntoFloat(volumeInDb) 
                : DefaultVolume
            ;
        }

        private string GetParameterName(AudioSourceType audioSourceType)
        {
            return audioSourceType switch
            {
                AudioSourceType.Music => "Music",
                AudioSourceType.SFX => "SFX",
                _ => throw new ArgumentOutOfRangeException(nameof(audioSourceType), audioSourceType, null)
            };
        }

        private float TransformVolumeDBIntoFloat(float volumeInDb) => Mathf.Pow(10f, volumeInDb / 20f);
    }
}