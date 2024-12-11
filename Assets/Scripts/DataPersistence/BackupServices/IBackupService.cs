namespace RehvidGames.DataPersistence.BackupServices
{
    public interface IBackupService
    {
        public bool RestoreBackup(string filePath);
        public void CreateBackup(string filePath);
    }
}