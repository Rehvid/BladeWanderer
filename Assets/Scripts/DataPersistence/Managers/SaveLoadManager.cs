namespace RehvidGames.DataPersistence.Managers
{
    using System.Collections.Generic;
    using RehvidGames.DataPersistence.Data;
    using RehvidGames.Interfaces;
    using UnityEngine;

    public class SaveLoadManager
    {
        private readonly IDataHandler _dataHandler;
        private readonly GameDataManager _gameDataManager;
        private readonly ProfileManager _profileManager;

        public SaveLoadManager(
            IDataHandler dataHandler, 
            GameDataManager gameDataManager, 
            ProfileManager profileManager
        ) {
            _dataHandler = dataHandler;
            _gameDataManager = gameDataManager;
            _profileManager = profileManager;
        }
        
        public void LoadGame(bool initializeDataIfNull, List<IDataPersistence> persistenceObjects)
        {
            GameData loadedData = _dataHandler.Load(_profileManager.GetCurrentProfileId());
            
            if (loadedData == null && initializeDataIfNull)
            {
                _gameDataManager.ResetGameData();
            }
            
            if (loadedData == null)
            {
                Debug.Log("No data was found. Initializing new game...");
                return;
            }

            foreach (var persistenceObject in persistenceObjects)
            {
                persistenceObject.LoadData(loadedData);
            }
        }
        
        public void SaveGame(List<IDataPersistence> persistenceObjects)
        {
            if (_gameDataManager.GetGameData() == null) return;
            GameData gameData = _gameDataManager.GetGameData();
            
            foreach (var persistenceObject in persistenceObjects)
            {
                persistenceObject.SaveData(gameData);
            }
         
            _dataHandler.Save(UpdateGameMetaData(gameData), _profileManager.GetCurrentProfileId());
        }

        private GameData UpdateGameMetaData(GameData gameData)
        {
            GameMetaData gameMetaData = gameData.GameMetaData;
            gameMetaData.UpdateLastUpdated();
            gameMetaData.UpdateCurrentSceneName();
            gameMetaData.ProfileId = _profileManager.GetCurrentProfileId();
            
            gameData.GameMetaData = gameMetaData;
            return gameData;
        }
    }
}