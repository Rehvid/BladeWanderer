namespace RehvidGames.DataPersistence.Data
{
    using System.Linq;
    using UnityEngine;

    [System.Serializable]
    public class GameGameplaySettings
    {
        private const int HighGraphicQualityIndex = 0;
        
        public bool IsFullScreen = true;
        public int ResolutionIndex;
        public int QualityIndex = HighGraphicQualityIndex;

        public GameGameplaySettings()
        {
            ResolutionIndex = Screen.resolutions
                .Select((resolution, index) => new { resolution, index })
                .OrderByDescending(res => res.resolution.width * res.resolution.height)
                .FirstOrDefault()?.index ?? 0;
        }
    }
}