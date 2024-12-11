namespace RehvidGames.DataPersistence.Data.State
{
    using Interfaces;

    [System.Serializable]
    public class GameState: ISavableData
    {
        public PlayerProfile PlayerProfile = new();
        public GameSessionData SessionData = new();
    }
}