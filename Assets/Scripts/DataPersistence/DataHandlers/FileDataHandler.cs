namespace RehvidGames.DataPersistence.DataHandlers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using BackupServices;
    using Data;
    using Data.Configuration;
    using Data.State;
    using Interfaces;
    using Serializers;
    using UnityEngine;

    public class FileDataHandler: IDataHandler
    {
        private readonly string _dataDirPath;
        private readonly string _dataFileName;
        private readonly ISerializer _serializer;
        private readonly IBackupService _backupService;
        
        private bool _isAllowedToRestoreDataFromBackup = true;
        
        public FileDataHandler(
            string dataDirPath, 
            string dataFileName, 
            ISerializer serializer,
            IBackupService backupService)
        {
            _dataDirPath = dataDirPath;
            _dataFileName = dataFileName;
            _serializer = serializer;
            _backupService = backupService;
        }
        
        #region Load
        public GameConfiguration LoadGameConfiguration() => LoadData<GameConfiguration>(GetFullPath());
        
        public Dictionary<string, GameState> LoadAllProfiles()
        {
            var profileDirectory = new Dictionary<string, GameState>();
            
            IEnumerable<DirectoryInfo> dirInfos = new DirectoryInfo(_dataDirPath).EnumerateDirectories();
            
            foreach (var dirInfo in dirInfos)
            {
                string profileId = dirInfo.Name;
                if (!File.Exists(GetFullPath(profileId))) continue;
                
                GameState profileData = LoadGameState(profileId);
                
                if (profileData == null) continue;
                
                profileDirectory.Add(profileId, profileData);
            }
            
            return profileDirectory;
        }

        public GameState LoadGameState(string profileId) => LoadData<GameState>(GetFullPath(profileId));
        
        private T LoadData<T>(string path) where T : ISavableData
        {
            if (!File.Exists(path)) return default;

            return TryDeserializeData<T>(path) ?? TryRestoreFromBackup<T>(path);
        }

        private T TryDeserializeData<T>(string path) where T : ISavableData
        {
            try
            {
                return _serializer.Deserialize<T>(path);
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to load data from file: " + path + "\n" + e);
                return default;
            }
        }

        private T TryRestoreFromBackup<T>(string path) where T : ISavableData
        {
            if (!_isAllowedToRestoreDataFromBackup) return default;
            
            try
            {
                return TryToLoadProfileDataFromBackup<T>(path);
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to load data from backup file: " + path + "\n" + e);
                return default;
            }
        }
        
        private T TryToLoadProfileDataFromBackup<T>(string fullPath) where T : ISavableData
        {
            Debug.LogWarning("Attempting to load data from backup...");
            var loadedData = default(T);
            
            if (_backupService.RestoreBackup(fullPath))
            {
                loadedData = LoadData<T>(fullPath);
            }

            _isAllowedToRestoreDataFromBackup = false;
            return loadedData;
        }
        
        #endregion
        
        #region Save game
        public void SaveGameConfiguration(GameConfiguration configuration) => SaveData(GetFullPath(), configuration);

        public void SaveGameState(GameState state, string profileId)
        {
            string fullPath = GetFullPath(profileId);
            EnsureDirectoryExists(fullPath);
            SaveData(fullPath, state);
        }
        
        private void EnsureDirectoryExists(string path)
        {
            var directoryPath = Path.GetDirectoryName(path) ?? string.Empty;
            Directory.CreateDirectory(directoryPath);
        }
        
        private void SaveData<T>(string path, T data) where T : ISavableData
        {
            try
            {
                _serializer.Serialize(path, data);
                TryToCreateBackup<T>(path);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error occurred during save operation: {path}\n{e}");
            }
        }
        
        private void TryToCreateBackup<T>(string fullPath) where T : ISavableData
        {
            try
            {
                var verifiedGameData = LoadData<T>(fullPath);
                
                if (verifiedGameData == null)
                {
                    throw new Exception("Verification failed: Loaded data is null.");
                }
                
                _backupService.CreateBackup(fullPath);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to create backup. Exception: {e}");
                throw;
            }
        }

        #endregion
        
        public void Delete(string profileId = null)
        {
            string fullPath = GetFullPath(profileId);
            if (!File.Exists(fullPath)) return;

            TryToDelete(fullPath);
        }

        private void TryToDelete(string fullPath)
        {
            try
            {
                Directory.Delete(Path.GetDirectoryName(fullPath) ?? string.Empty, true);
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to delete data from file: " + fullPath + "\n" + e);
            }
        }
        
        public string GetMostRecentlyUpdatedProfileId()
        {
            string mostRecentProfileId = null;
            Dictionary<string, GameState> profiles = LoadAllProfiles();
            
            foreach (KeyValuePair<string, GameState> profile in profiles)
            {
                string profileId = profile.Key;
                GameState gameData = profile.Value;
                
                if (gameData == null) continue;

                if (mostRecentProfileId == null)
                {
                    mostRecentProfileId = profileId;
                }
                else
                {
                    DateTime mostRecentDate = profiles[mostRecentProfileId].SessionState.GetDateTimeFromLastUpdated();
                    DateTime newDateTime = gameData.SessionState.GetDateTimeFromLastUpdated();
                    
                    if (newDateTime > mostRecentDate)
                    {
                        mostRecentProfileId = profileId;
                    }
                }
            }
            return mostRecentProfileId;
        }
        
        private string GetFullPath(string profileId = null)
        {
            if (string.IsNullOrEmpty(profileId))
            {
                return Path.Combine(_dataDirPath, _dataFileName);
            }
            
            return Path.Combine(_dataDirPath, profileId, _dataFileName);
        }
    }
}