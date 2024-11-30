namespace RehvidGames.DataPersistence
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Interfaces;
    using Serializable;
    using UnityEngine;

    public class FileDataHandler: IDataHandler
    {
        private readonly string _dataDirPath;
        
        private readonly string _dataFileName;
        
        private bool _useEncryption = false;
        
        private readonly string _encryptionCodeWord = "crocodile";
        private readonly string _backupExtension = ".bak";
        
        public FileDataHandler(string dataDirPath, string dataFileName, bool useEncryption)
        {
            _dataDirPath = dataDirPath;
            _dataFileName = dataFileName;
            _useEncryption = useEncryption;
           
        }

        public GameData Load(string profileId)
        {
            if (profileId == null)
            {
                return null;
            }
            
            return LoadDataFromFile(profileId);
        }

        public Dictionary<string, GameData> LoadAllProfiles()
        {
            var profileDirectory = new Dictionary<string, GameData>();
            IEnumerable<DirectoryInfo> dirInfos = new DirectoryInfo(_dataDirPath).EnumerateDirectories();
            foreach (DirectoryInfo dirInfo in dirInfos)
            {
                string profileId = dirInfo.Name;
                string fullPath = Path.Combine(_dataDirPath, profileId, _dataFileName);
                if (!File.Exists(fullPath)) continue;
                
                GameData profileData = Load(profileId);
                if (profileData == null) continue;
                
                profileDirectory.Add(profileId, profileData);
            }
            
            return profileDirectory;
        }

        private GameData LoadDataFromFile(string profileId, bool allowRestoreFromBackup = true)
        {
            GameData loadedData = null;
            string fullPath = Path.Combine(_dataDirPath, profileId, _dataFileName);
            
            if (!File.Exists(fullPath)) return null;
            
            try
            {
                string dataToLoad = File.ReadAllText(fullPath);
                
                if (_useEncryption)
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }
                
                loadedData =  JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogWarning("Error occured when trying to load data from file: " + fullPath + "\n" + e);
                if (allowRestoreFromBackup)
                {
                     bool rollbackSuccess = AttemptRollback(fullPath);
                    if (rollbackSuccess)
                    {
                        loadedData = LoadDataFromFile(profileId, false);
                    }
                }
                else
                {
                    Debug.LogError("error occured when trying to load file at path " + fullPath + "\n" + "and back up did not work" + "\n"  + e);
                }
               
            }

            return loadedData;
        }

        public void Save(GameData data, string profileId)
        {
            if (profileId == null)
            {
                return;
            }
            
            string fullPath = Path.Combine(_dataDirPath, profileId, _dataFileName);
            string backupFilePath = fullPath + _backupExtension;
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath) ?? string.Empty);
                
                string dataToStore = JsonUtility.ToJson(data, true);

                if (_useEncryption)
                {
                    dataToStore = EncryptDecrypt(dataToStore);
                }
                
                File.WriteAllText(fullPath, dataToStore);
                
                GameData verifiedGameData = Load(profileId);
                if (verifiedGameData != null)
                {
                    File.Copy(fullPath, backupFilePath, true);
                }
                else
                {
                    throw new Exception("Save file could not be verified and backup could not be created.");
                }
                
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e);
            }
        }

        public void Delete(string profileId)
        {
            if (profileId == null) return;
            
            string fullPath = Path.Combine(_dataDirPath, profileId, _dataFileName);
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
        
        public string GetMostRecentlyUpdatedProfileId()
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
                    DateTime mostRecentDate = DateTime.FromBinary(profiles[mostRecentProfileId].LastUpdated);
                    DateTime newDateTime = DateTime.FromBinary(gameData.LastUpdated);
                    if (newDateTime > mostRecentDate)
                    {
                        mostRecentProfileId = profileId;
                    }
                }
            }
            return mostRecentProfileId;
        }

        // Simple implementation of XOR encryption;
        private string EncryptDecrypt(string dataToEncrypt)
        {
            string modifedData = "";
            for (int i = 0; i < dataToEncrypt.Length; i++)
            {
                modifedData += (char) (dataToEncrypt[i] ^ _encryptionCodeWord[i % _encryptionCodeWord.Length]);
            }
            return modifedData;
        }

        private bool AttemptRollback(string fullpath)
        {
            bool success = false;
            string backupFilePath = fullpath + _backupExtension;
            try
            {
                if (File.Exists(backupFilePath))
                {
                    File.Copy(backupFilePath, fullpath, true);
                    success = true;
                    Debug.LogWarning("Had to roll back to backup file at: " + backupFilePath);
                }
                else
                {
                    throw new Exception("Tried to roll back, but no backup file exists to rolL BACK TO: ");
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to roll backup file at :" + backupFilePath + "\n" + e);
            }
            
            return success;
        }
    }
}