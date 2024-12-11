namespace RehvidGames.UI.Menu
{
    using System.Collections.Generic;
    using System.Linq;
    using DataPersistence.Data;
    using DataPersistence.Data.Configuration;
    using DataPersistence.Managers;
    using Interfaces;
    using Managers;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class GameplayMenu : MonoBehaviour, IDataPersistence<GameConfiguration>
    {
        [Header("Gameplay interactable options")]
        [SerializeField] private TMP_Dropdown _resolutionDropdown;
        [SerializeField] private TMP_Dropdown _qualityDropdown;
        [SerializeField] private Toggle _fullscreenToggle;
        
        private Resolution[] _resolutions;
        
        private void OnDestroy()
        {
            RegistryManager<IDataPersistence<GameConfiguration>>.Instance?.Unregister(this);
        }

        private void Start()
        {
            if (_resolutions == null)
            {
                InitializeResolutions();
            }
        }

        private void InitializeResolutions()
        {
            _resolutions = Screen.resolutions;

            List<string> options = _resolutions.Select(resolution => resolution.width + " x " + resolution.height)
                .Distinct()
                .ToList();
            
            _resolutionDropdown.ClearOptions();
            _resolutionDropdown.AddOptions(options);
        }
        
        
        public void OnQualityChanged(int qualityIndex) => SetQualitySettingsLevel(qualityIndex);
        

        private void SetQualitySettingsLevel(int qualityIndex)
        {
            QualitySettings.SetQualityLevel(qualityIndex);
        }
        
        public void OnResolutionChanged(int resolutionIndex) => SetScreenResolution(resolutionIndex);

        private void SetScreenResolution(int resolutionIndex)
        {
            Resolution resolution = _resolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }
        
        public void OnFullScreenChanged(bool isFullScreen) => SetFullScreen(isFullScreen);
        
        private void SetFullScreen(bool isFullScreen) => Screen.fullScreen = isFullScreen;
        
        public void LoadData(GameConfiguration data)
        {
            GameplaySettings settings = data.GameplaySettings;
            if (_resolutions == null)
            {
                InitializeResolutions();
            }
            
            SetFullScreen(settings.IsFullScreen);
            _fullscreenToggle.isOn = settings.IsFullScreen;
             
            SetScreenResolution(settings.ResolutionIndex);
            _resolutionDropdown.value = settings.ResolutionIndex;
            _resolutionDropdown.RefreshShownValue();
            
            SetQualitySettingsLevel(settings.QualityIndex);
            _qualityDropdown.value = settings.QualityIndex;
            _qualityDropdown.RefreshShownValue();
        }

        public void SaveData(GameConfiguration data)
        {
            GameplaySettings gameplaySettings = data.GameplaySettings;
            
            gameplaySettings.IsFullScreen = _fullscreenToggle.isOn;
            gameplaySettings.ResolutionIndex = _resolutionDropdown.value;
            gameplaySettings.QualityIndex = QualitySettings.GetQualityLevel();
            
            data.GameplaySettings = gameplaySettings;
        }
    }
}