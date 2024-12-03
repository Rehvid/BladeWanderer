namespace RehvidGames.DataPersistence.DataHandlers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Data;
    using UnityEngine;

    public class FileDataHandler: BaseDataHandler
    {
        private readonly string _dataDirPath;
        
        private readonly string _dataFileName;
        
        private readonly bool _useEncryption;
        
        public FileDataHandler(string dataDirPath, string dataFileName, bool useEncryption) : base(useEncryption)
        {
            _dataDirPath = dataDirPath;
            _dataFileName = dataFileName;
            _useEncryption = useEncryption;
        }

        #region Load game
        public override Dictionary<string, GameData> LoadAllProfiles()
        {
            var profileDirectory = new Dictionary<string, GameData>();
            
            IEnumerable<DirectoryInfo> dirInfos = new DirectoryInfo(_dataDirPath).EnumerateDirectories();
            
            foreach (var dirInfo in dirInfos)
            {
                string profileId = dirInfo.Name;
                if (!File.Exists(GetFullPath(profileId))) continue;
                
                GameData profileData = Load(profileId);
                
                if (profileData == null) continue;
                
                profileDirectory.Add(profileId, profileData);
            }
            
            return profileDirectory;
        }
        
        public override GameData Load(string profileId)
        {
            string fullPath = GetFullPath(profileId);
            if (!File.Exists(fullPath)) return null;

            try
            {
                return TryToLoadProfileDataFromFile(fullPath);
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to load data from file: " + fullPath + "\n" + e);
            }
            
            if (!isAllowedToRestoreDataFromBackup) return null;
            
            try
            {
                return TryToLoadProfileDataFromBackup(fullPath, profileId);
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to load data from backup file: " + fullPath + "\n" + e);
                return null;
            }
        }
        
        private GameData TryToLoadProfileDataFromFile(string fullPath)
        {
            try
            {
                string dataToLoad = File.ReadAllText(fullPath);

                if (_useEncryption)
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }

                return JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception ex)
            {
                Debug.LogWarning("Error occured when trying to load data from file: " + fullPath + "\n" + ex);
                throw;
            }
        }

        private GameData TryToLoadProfileDataFromBackup(string fullPath, string profileId)
        {
            Debug.LogWarning("Attempting to load data from backup...");
            GameData loadedData = null;
            
            if (AttemptRollback(fullPath))
            {
                loadedData = Load(profileId);
            }
            
            ResetBackupPermission();
            return loadedData;
        }
        
        private bool AttemptRollback(string fullPath)
        {
            string backupFilePath = GetFullPath(fullPath);
            
            if (!File.Exists(backupFilePath))
            {
                Debug.LogWarning($"Rollback failed: no backup file found at path: {backupFilePath}");
                return false;
            }
            
            try
            {
                File.Copy(backupFilePath, fullPath, true);
                Debug.LogWarning("Had to roll back to backup file at: " + backupFilePath);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to roll backup file at :" + backupFilePath + "\n" + e);
                return false;
            }
        }
        
        
        #endregion
        
        #region Save game
        public override void Save(GameData data, string profileId)
        {
            string fullPath = GetFullPath(profileId);
            
            if (string.IsNullOrEmpty(fullPath)) return;
            
            string backupFilePath = GetBackupPath(fullPath);
            
            try
            {
                TryToSaveProfileData(data, fullPath, profileId);
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e);
            }

            try
            {
                TryToCreateBackup(profileId, fullPath, backupFilePath);
            }
            catch (Exception e)
            {
                Debug.LogError($"Save succeeded, but creating a backup failed: {backupFilePath}\n{e}");
            }
        }

        private void TryToSaveProfileData(GameData data, string fullPath, string profileId)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath) ?? string.Empty);
                    
                string dataToStore = JsonUtility.ToJson(data, true);

                if (_useEncryption)
                {
                    dataToStore = EncryptDecrypt(dataToStore);
                }
                    
                File.WriteAllText(fullPath, dataToStore);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to save data for profile {profileId} at {fullPath}. Exception: {e}");
                throw;
            }
        }
        
        private void TryToCreateBackup(string profileId, string fullPath, string backupFilePath)
        {
            try
            {
                GameData verifiedGameData = Load(profileId);
                
                if (verifiedGameData == null)
                {
                    throw new Exception("Verification failed: Loaded data is null.");
                }
                
                File.Copy(fullPath, backupFilePath, true);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to create backup for profile {profileId}. Exception: {e}");
                throw;
            }
        }

        #endregion
        
        public override void Delete(string profileId)
        {
            string fullPath = GetFullPath(profileId);
            if (!File.Exists(fullPath)) return;

            try
            {
                Directory.Delete(Path.GetDirectoryName(fullPath) ?? string.Empty, true);
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to delete data from file: " + fullPath + "\n" + e);
            }
        }
        
        public override string GetMostRecentlyUpdatedProfileId()
        {
            string mostRecentProfileId = null;
            Dictionary<string, GameData> profiles = LoadAllProfiles();
            
            foreach (KeyValuePair<string, GameData> profile in profiles)
            {
                string profileId = profile.Key;
                GameData gameData = profile.Value;
                
                if (gameData == null) continue;

                if (mostRecentProfileId == null)
                {
                    mostRecentProfileId = profileId;
                }
                else
                {
                    DateTime mostRecentDate = profiles[mostRecentProfileId].GameMetaData.GetDateTimeFromLastUpdated();
                    DateTime newDateTime = gameData.GameMetaData.GetDateTimeFromLastUpdated();
                    
                    if (newDateTime > mostRecentDate)
                    {
                        mostRecentProfileId = profileId;
                    }
                }
            }
            return mostRecentProfileId;
        }
        
        private string GetFullPath(string profileId)
        {
            return string.IsNullOrEmpty(profileId) ? null : Path.Combine(_dataDirPath, profileId, _dataFileName);
        }
    }
}