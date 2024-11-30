namespace RehvidGames.Interfaces
{
    using Serializable;

    public interface IDataPersistence
    {
        public void LoadData(GameData data);
        public void SaveData(GameData data);
    }
}