namespace RehvidGames.DataPersistence.DataHandlers
{
    using System.Collections.Generic;
    using Data.Configuration;
    using Data.State;

    public interface IDataHandler
    {
        public void SaveGameConfiguration(GameConfiguration configuration);
        public GameConfiguration LoadGameConfiguration();
        public GameState LoadGameState(string profileId);
        public void SaveGameState(GameState state, string profileId);
        public void Delete(string profileId);
        public string GetMostRecentlyUpdatedProfileId();
        /// <summary>Loads all saved profiles from storage.</summary>
        /// <returns>A dictionary of profile IDs and their corresponding game data.</returns>
        public Dictionary<string, GameState> LoadAllProfiles();
    }
}