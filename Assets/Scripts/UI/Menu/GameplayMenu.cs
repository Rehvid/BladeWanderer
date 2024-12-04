namespace RehvidGames.UI.Menu
{
    using System.Collections.Generic;
    using System.Linq;
    using DataPersistence.Data;
    using DataPersistence.Managers;
    using Interfaces;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class GameplayMenu : MonoBehaviour, IGameSettingsPersistence
    {
        [Header("Gameplay interactable options")]
        [SerializeField] private TMP_Dropdown _resolutionDropdown;
        [SerializeField] private TMP_Dropdown _qualityDropdown;
        [SerializeField] private Toggle _fullscreenToggle;
        
        private Resolution[] _resolutions;
        
        private void Awake()
        {
            GameSettingsPersistenceRegistryManager.Instance.Register(this);
        }

        private void OnDestroy()
        {
            GameSettingsPersistenceRegistryManager.Instance.Unregister(this);
        }

        private void Start()
        {
            InitializeResolutions();
            SetCurrentResolution();
            GameSettingsPersistenceManager.Instance.LoadGameSettings();
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

        private void SetCurrentResolution()
        {
            int currentResolutionIndex = _resolutions
                .Select((resolution, index) => new { resolution, index })
                .FirstOrDefault(item => item.resolution.width == Screen.currentResolution.width &&
                                        item.resolution.height == Screen.currentResolution.height)
                ?.index ?? 0;
            
            _resolutionDropdown.value = currentResolutionIndex;
            _resolutionDropdown.RefreshShownValue();
        }

        public void OnQualityChanged(int qualityIndex)
        {
            QualitySettings.SetQualityLevel(qualityIndex);
        }

        public void OnResolutionChanged(int resolutionIndex)
        {
            Resolution resolution = _resolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }

        public void OnFullScreenChanged(bool isFullScreen)
        {
            Screen.fullScreen = isFullScreen;
        }

        public void LoadGameSettings(GameSettings settings)
        {
            GameGameplaySettings gameplaySettings = settings.GameplaySettings;
            
            _resolutionDropdown.value = gameplaySettings.ResolutionIndex;
            _qualityDropdown.value = gameplaySettings.QualityIndex;
            _fullscreenToggle.isOn = gameplaySettings.IsFullScreen;
        }

        public void SaveGameSettings(GameSettings settings)
        {
            GameGameplaySettings gameplaySettings = settings.GameplaySettings;
            
            gameplaySettings.IsFullScreen = Screen.fullScreen;
            gameplaySettings.ResolutionIndex = _resolutionDropdown.value;
            gameplaySettings.QualityIndex = QualitySettings.GetQualityLevel();
            
            settings.GameplaySettings = gameplaySettings;
        }
    }
}