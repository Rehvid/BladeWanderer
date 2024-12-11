namespace RehvidGames.DataPersistence.Managers
{
    using System.Collections.Generic;
    using Data.State;
    using DataHandlers;
    
    public class ProfileManager
    {
        private readonly IDataHandler _dataHandler;
        private string _currentProfileId;

        public ProfileManager(IDataHandler dataHandler)
        {
            _dataHandler = dataHandler;
            _currentProfileId = _dataHandler.GetMostRecentlyUpdatedProfileId();
        }
        
        public string GetCurrentProfileId()
        {
            return _currentProfileId;
        }

        public void SetCurrentProfile(string newProfileId)
        {
            _currentProfileId = newProfileId;
        }

        public void DeleteProfile(string profileId)
        {
            _dataHandler.Delete(profileId);
            SetCurrentProfile(_dataHandler.GetMostRecentlyUpdatedProfileId());
        }

        public Dictionary<string, GameState> GetAllProfiles()
        {
            return _dataHandler.LoadAllProfiles();
        }
        
        
    }
}