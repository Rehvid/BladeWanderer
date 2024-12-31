namespace RehvidGames.Interfaces
{
    using DataPersistence.Data;
    
    public interface IDataPersistence<T>
    {
        public void LoadData(T data);
        public void SaveData(T data);
    }
}