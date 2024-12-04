namespace RehvidGames.Interfaces
{
    using System.Collections.Generic;
    using DataPersistence.Data;
    using Serializable;

    public interface IDataHandler
    {
        public void SaveSettings(GameSettings settings);
        public GameSettings LoadSettings();
        public GameData Load(string profileId);
        public void Save(GameData data, string profileId);
        public void Delete(string profileId);
        public string GetMostRecentlyUpdatedProfileId();
        /// <summary>Loads all saved profiles from storage.</summary>
        /// <returns>A dictionary of profile IDs and their corresponding game data.</returns>
        public Dictionary<string, GameData> LoadAllProfiles();
        
    }
}