namespace RehvidGames.Interfaces
{
    using DataPersistence.Data;
    using Serializable;

    public interface IDataPersistence
    {
        public void LoadData(GameData data);
        public void SaveData(GameData data);
    }
}