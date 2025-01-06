namespace RehvidGames.DataPersistence.Interfaces
{
    
    public interface IDataPersistence<T>
    {
        public void LoadData(T data);
        public void SaveData(T data);
    }
}