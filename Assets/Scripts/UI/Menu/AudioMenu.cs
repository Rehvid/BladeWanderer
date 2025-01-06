namespace RehvidGames.UI.Menu
{
    using DataPersistence.Data.Configuration;
    using DataPersistence.Interfaces;
    using Managers;
    using UnityEngine;
    using UnityEngine.Audio;
    using UnityEngine.UI;
    using AudioSettings = DataPersistence.Data.Configuration.AudioSettings;

    public class AudioMenu : MonoBehaviour, IDataPersistence<GameConfiguration>
    {
        [SerializeField] private AudioMixer _audioMixer;
        
        [Header("Audio Sliders")]
        [SerializeField] private Slider _masterVolumeSlider;
        [SerializeField] private Slider _musicVolumeSlider;
        [SerializeField] private Slider _sfxVolumeSlider;
        
        private void OnDestroy()
        {
            RegistryManager<IDataPersistence<GameConfiguration>>.Instance?.Unregister(this);
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
        
        public void LoadData(GameConfiguration configuration)
        {
            AudioSettings settings = configuration.AudioSettings;
            
            _masterVolumeSlider.value = settings.MasterVolume;
            _musicVolumeSlider.value = settings.MusicVolume;
            _sfxVolumeSlider.value = settings.SfxVolume;
        }

        public void SaveData(GameConfiguration configuration)
        {
            AudioSettings settings = configuration.AudioSettings;
            
            _audioMixer.GetFloat("MasterVolume", out settings.MasterVolume);
            _audioMixer.GetFloat("Music", out settings.MusicVolume);
            _audioMixer.GetFloat("SFX", out settings.SfxVolume);
            
            configuration.AudioSettings = settings;
        }
    }
}