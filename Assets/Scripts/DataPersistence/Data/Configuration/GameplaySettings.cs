namespace RehvidGames.DataPersistence.Data.Configuration
{
    using System.Linq;
    using UnityEngine;

    [System.Serializable]
    public class GameplaySettings
    {
        private const int HighGraphicQualityIndex = 0;
        
        public bool IsFullScreen = true;
        public int ResolutionIndex;
        public int QualityIndex = HighGraphicQualityIndex;

        public GameplaySettings()
        {
            ResolutionIndex = Screen.resolutions
                .Select((resolution, index) => new { resolution, index })
                .OrderByDescending(res => res.resolution.width * res.resolution.height)
                .FirstOrDefault()?.index ?? 0;
        }
    }
}