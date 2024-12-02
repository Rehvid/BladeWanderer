namespace RehvidGames.DataPersistence.Data
{
    [System.Serializable]
    public class GameData
    {
        public PlayerGameData PlayerGameData = new();
        public GameMetaData GameMetaData = new();
    }
}