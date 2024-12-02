namespace RehvidGames.DataPersistence.DataHandlers
{
    using System.Collections.Generic;
    using Data;
    using Interfaces;

    public abstract class BaseDataHandler: IDataHandler
    {
        private const string EncryptionCodeWord = "crocodile";
        private const string BackupExtension = ".backup";
        
        protected readonly bool useEncryption;
        protected bool isAllowedToRestoreDataFromBackup = true;

        #region Interfaces methods
         public abstract GameData Load(string profileId);
        public abstract void Save(GameData data, string profileId);
        public abstract void Delete(string profileId);
        public abstract string GetMostRecentlyUpdatedProfileId();
        public abstract Dictionary<string, GameData> LoadAllProfiles();
        #endregion
       
        protected BaseDataHandler(bool useEncryption)
        {
            this.useEncryption = useEncryption;
        }
        
        protected void ResetBackupPermission()
        {
            isAllowedToRestoreDataFromBackup = false;
        }

        protected string GetBackupPath(string fullPath)
        {
            return fullPath + BackupExtension;
        }
        
        /// <summary>
        /// Encrypts or decrypts a given string by applying an XOR operation 
        /// with a repeating encryption key. This method works symmetrically,
        /// meaning the same function can be used for both encryption and decryption.
        /// </summary>
        protected string EncryptDecrypt(string dataToEncrypt)
        {
            string modifiedData = "";
            for (int i = 0; i < dataToEncrypt.Length; i++)
            {
                modifiedData += (char) (dataToEncrypt[i] ^ EncryptionCodeWord[i % EncryptionCodeWord.Length]);
            }
            return modifiedData;
        }
    }
}