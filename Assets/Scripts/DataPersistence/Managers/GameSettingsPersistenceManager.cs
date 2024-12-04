namespace RehvidGames.DataPersistence.Managers
{
    using System;
    using System.Collections.Generic;
    using DataHandlers;
    using Interfaces;
    using UnityEngine;

    public class GameSettingsPersistenceManager: MonoBehaviour
    {
        public static GameSettingsPersistenceManager Instance { get; private set; }

        [SerializeField] private bool _useEncryption; 
        
        private GameSettingsManager _gameSettingsManager;
        private IDataHandler _dataHandler;
        private List<IGameSettingsPersistence> _persistenceList;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            _persistenceList = GameSettingsPersistenceRegistryManager.Instance.GetAllRegisteredObjects();
            InstanceComponents();
        }

        private void InstanceComponents()
        {
            _gameSettingsManager = new GameSettingsManager();
            _dataHandler = new FileDataHandler(
                Application.persistentDataPath, 
                "settings.game", 
                _useEncryption
            );
        }

        public void SaveGameSettings()
        {
            var gameSettings = _gameSettingsManager.GameSettings;
            foreach (IGameSettingsPersistence persistence in _persistenceList)
            {
                persistence.SaveGameSettings(gameSettings);
            }  
            
            _dataHandler.SaveSettings(gameSettings);
        }

        public void LoadGameSettings()
        {
            _gameSettingsManager.GameSettings = _dataHandler.LoadSettings();
            foreach (IGameSettingsPersistence persistence in _persistenceList)
            {
                persistence.LoadGameSettings(_gameSettingsManager.GameSettings);
            }
        }
        
        
        private void OnApplicationQuit()
        {
            SaveGameSettings();
        }
    }
}