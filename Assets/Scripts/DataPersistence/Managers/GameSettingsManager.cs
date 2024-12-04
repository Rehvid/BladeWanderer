namespace RehvidGames.DataPersistence.Managers
{
    using Data;

    public class GameSettingsManager
    {
        public GameSettings GameSettings { get; set; } = new();

        public void ResetGameSettings()
        {
            GameSettings = new GameSettings();
        }
    }
}