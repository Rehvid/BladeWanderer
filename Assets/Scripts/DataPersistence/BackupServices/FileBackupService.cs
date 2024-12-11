namespace RehvidGames.DataPersistence.BackupServices
{
    using System;
    using System.IO;
    using UnityEngine;

    public class FileBackupService: IBackupService
    {
        private const string BackupExtension = ".backup";
        
        
        public bool RestoreBackup(string filePath)
        {
            string backupFilePath = GetBackupPath(filePath);
            
            if (!File.Exists(backupFilePath))
            {
                Debug.LogWarning($"Rollback failed: no backup file found at path: {backupFilePath}");
                return false;
            }
            
            try
            {
                File.Copy(backupFilePath, filePath, true);
                Debug.LogWarning("Had to roll back to backup file at: " + backupFilePath);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to roll backup file at :" + backupFilePath + "\n" + e);
                return false;
            }
        }

        
        public void CreateBackup(string fullPath)
        {
            try
            {
                File.Copy(fullPath, GetBackupPath(fullPath), true);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to create backup. Exception: {e}");
                throw;
            }
        }
        
        private string GetBackupPath(string fullPath) => fullPath + BackupExtension;
    }
}