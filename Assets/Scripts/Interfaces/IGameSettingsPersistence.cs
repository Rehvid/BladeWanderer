namespace RehvidGames.Interfaces
{
    using DataPersistence.Data;

    public interface IGameSettingsPersistence
    {
        public void LoadGameSettings(GameSettings settings);
        public void SaveGameSettings(GameSettings settings);
    }
}