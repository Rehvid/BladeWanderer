namespace RehvidGames.UI.Menu
{
    using System;
    using DataPersistence.Data;
    using DataPersistence.Managers;
    using Interfaces;
    using UnityEngine;
    using UnityEngine.Audio;
    using UnityEngine.UI;

    public class AudioMenu : MonoBehaviour, IGameSettingsPersistence
    {
        [SerializeField] private AudioMixer _audioMixer;
        
        [Header("Audio Sliders")]
        [SerializeField] private Slider _masterVolumeSlider;
        [SerializeField] private Slider _musicVolumeSlider;
        [SerializeField] private Slider _sfxVolumeSlider;
        
        private void Awake()
        {
            Debug.Log("Audio Menu Awake");
            GameSettingsPersistenceRegistryManager.Instance.Register(this);
        }

        private void OnDestroy()
        {
            GameSettingsPersistenceRegistryManager.Instance.Unregister(this);
        }

        private void Start()
        {
            GameSettingsPersistenceManager.Instance.LoadGameSettings();
        }

        public void OnMasterVolume(float value)
        {
            _audioMixer.SetFloat("MasterVolume", value);
        }

        public void OnMusicVolume(float value)
        {
            _audioMixer.SetFloat("Music", value);
        }

        public void OnSFXVolume(float value)
        {
            _audioMixer.SetFloat("SFX", value);
        }
        
        public void LoadGameSettings(GameSettings settings)
        {
            _masterVolumeSlider.value = settings.GameAudioSettings.MasterVolume;
            _musicVolumeSlider.value = settings.GameAudioSettings.MusicVolume;
            _sfxVolumeSlider.value = settings.GameAudioSettings.SfxVolume;
        }

        public void SaveGameSettings(GameSettings settings)
        {
            GameAudioSettings audioSettings = settings.GameAudioSettings;
            
            _audioMixer.GetFloat("MasterVolume", out audioSettings.MasterVolume);
            _audioMixer.GetFloat("Music", out audioSettings.MusicVolume);
            _audioMixer.GetFloat("SFX", out audioSettings.SfxVolume);
            
            settings.GameAudioSettings = audioSettings;
        }
    }
}