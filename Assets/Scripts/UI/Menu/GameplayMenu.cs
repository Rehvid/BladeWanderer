namespace RehvidGames.UI.Menu
{
    using System.Collections.Generic;
    using System.Linq;
    using TMPro;
    using UnityEngine;

    public class GameplayMenu : MonoBehaviour
    {
        private Resolution[] _resolutions;
        [SerializeField] private TMP_Dropdown _resolutionDropdown;

        private void Start()
        {
            InitializeResolutions();
            SetCurrentResolution();
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
    }
}