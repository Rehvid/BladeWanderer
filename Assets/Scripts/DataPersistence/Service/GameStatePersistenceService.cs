namespace RehvidGames.DataPersistence.Service
{
    using System.Collections.Generic;
    using Managers;
    using Data.State;
    using DataHandlers;
    using Interfaces;
    using UnityEngine;

    public class GameStatePersistenceService: PersistenceService<GameState>
    {
        private readonly ProfileManager _profileManager;
        private readonly bool _hasInitializeDataIfNull;
        
        public GameStatePersistenceService(
            IDataHandler dataHandler, 
            DataManager<GameState> dataManager, 
            ProfileManager profileManager,
            bool hasInitializeDataIfNull
        ): base(dataHandler, dataManager) {
            _profileManager = profileManager;
            _hasInitializeDataIfNull = hasInitializeDataIfNull;
        }
        
        public override void Load(List<IDataPersistence<GameState>> persistenceObjects)
        {
            dataManager.Data = dataHandler.LoadGameState(_profileManager.GetCurrentProfileId());
            
            if (!dataManager.HasData() && _hasInitializeDataIfNull)
            {
                dataManager.ResetData();
            }
            
            if (!dataManager.HasData())
            {
                Debug.Log("No data was found. Initializing new game...");
                return;
            }

            LoadFromPersistenceObjects(persistenceObjects);
        }
        
        public override void Save(List<IDataPersistence<GameState>> persistenceObjects)
        {
            SaveToPersistenceObjects(persistenceObjects);
            
            dataHandler.SaveGameState(UpdateSessionData(dataManager.Data), _profileManager.GetCurrentProfileId());
        }

        private GameState UpdateSessionData(GameState state)
        {
            GameSessionState sessionState = state.SessionState;
            sessionState.UpdateLastUpdated();
            sessionState.UpdateCurrentSceneName();
            sessionState.UpdateSceneIndex();
            sessionState.ProfileId = _profileManager.GetCurrentProfileId();
            
            state.SessionState = sessionState;
            return state;
        }
    }
}